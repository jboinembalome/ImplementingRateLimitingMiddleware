using ImplementingRateLimitingMiddleware.Models;
using Microsoft.EntityFrameworkCore;

namespace ImplementingRateLimitingMiddleware.Persistence;

public sealed class CustomerDbContext(DbContextOptions<CustomerDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
}
