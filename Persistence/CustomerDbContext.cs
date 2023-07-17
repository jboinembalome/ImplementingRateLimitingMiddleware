using ImplementingRateLimitingMiddleware.Models;
using Microsoft.EntityFrameworkCore;

namespace ImplementingRateLimitingMiddleware.Persistence;

public class CustomerDbContext : DbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
}
