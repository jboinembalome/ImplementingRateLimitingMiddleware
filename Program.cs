using System.Globalization;
using System.Threading.RateLimiting;
using ImplementingRateLimitingMiddleware.Interfaces;
using ImplementingRateLimitingMiddleware.Persistence;
using ImplementingRateLimitingMiddleware.Persistence.Interfaces;
using ImplementingRateLimitingMiddleware.Persistence.Repositories;
using ImplementingRateLimitingMiddleware.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CustomerDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", options =>
    {
        options.PermitLimit = 10;
        options.Window = TimeSpan.FromSeconds(10);
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 5;
    });

    options.AddSlidingWindowLimiter("sliding", options =>
    {
        options.PermitLimit = 10;
        options.Window = TimeSpan.FromSeconds(10);
        options.SegmentsPerWindow = 2;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 5;
    });

    options.AddTokenBucketLimiter("token", options =>
    {
        options.TokenLimit = 100;
        options.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
        options.TokensPerPeriod = 20;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 5;
    });

    options.AddConcurrencyLimiter("concurrency", options =>
    {
        options.PermitLimit = 10;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        options.QueueLimit = 5;
    });

    // One fixed window per client IP address: a noisy client cannot use up everybody else's quota.
    options.AddPolicy("per-ip", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromSeconds(10)
            }));

    // Applies to every request, on top of the named policy of the endpoint (if any).
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, token) =>
    {
        string errorMessage;
        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            context.HttpContext.Response.Headers.RetryAfter =
                ((int)Math.Ceiling(retryAfter.TotalSeconds)).ToString(CultureInfo.InvariantCulture);

            errorMessage = "Too many requests. " +
                           $"Please try again after {retryAfter.TotalSeconds} second(s). " +
                           "You can read more information about rate limits at https://example.org/docs/ratelimiting.";

            await context.HttpContext.Response.WriteAsync(errorMessage, token);
            return;
        }

        errorMessage = "Too many requests. " +
                       "Please try again later. " +
                       "You can read more information about rate limits at https://example.org/docs/ratelimiting.";
        await context.HttpContext.Response.WriteAsync(errorMessage, token);
    };
});

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Initialize and seed database
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
        await CustomerDbContextSeed.SeedDataAsync(dbContext);
    }

    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

// GET: api/minimal/customers
app.MapGet("/api/minimal/customers", (ICustomerService customerService, CancellationToken cancellationToken) =>
        customerService.GetCustomersAsync(cancellationToken))
    .RequireRateLimiting("per-ip");

app.Run();
