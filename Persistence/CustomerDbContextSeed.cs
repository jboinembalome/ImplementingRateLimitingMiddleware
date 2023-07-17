using ImplementingRateLimitingMiddleware.Models;

namespace ImplementingRateLimitingMiddleware.Persistence;

public static class CustomerDbContextSeed
{
    public static async Task SeedDataAsync(CustomerDbContext dbContext)
    {
        await SeedCustomersAsync(dbContext);
    }

    private static async Task SeedCustomersAsync(CustomerDbContext dbContext)
    {
        var customers = new List<Customer>
        {
            new Customer
            {
                Id = Guid.Parse("31a7ffcf-d099-4637-bd58-2a87641d1aaf"),
                FirstName = "Jimmy",
                LastName = "Boinembalome",
                Email = "jboinembalome@gmail.com"
            },
            new Customer
            {
                Id = Guid.Parse("41a7ffcf-d099-4637-bd58-2a87641d1aaf"),
                FirstName = "Jimmy2",
                LastName = "Boinembalome",
                Email = "jboinembalome@gmail.com"
            },
            new Customer
            {
                Id = Guid.Parse("51a7ffcf-d099-4637-bd58-2a87641d1aaf"),
                FirstName = "Jimmy3",
                LastName = "Boinembalome",
                Email = "jboinembalome@gmail.com"
            },
            new Customer
            {
                Id = Guid.Parse("61a7ffcf-d099-4637-bd58-2a87641d1aaf"),
                FirstName = "Jimmy4",
                LastName = "Boinembalome",
                Email = "jboinembalome@gmail.com"
            },
            new Customer
            {
                Id = Guid.Parse("71a7ffcf-d099-4637-bd58-2a87641d1aaf"),
                FirstName = "Jimmy5",
                LastName = "Boinembalome",
                Email = "jboinembalome@gmail.com"
            },
        };

        await dbContext.Customers.AddRangeAsync(customers);
        await dbContext.SaveChangesAsync();
    }
}
