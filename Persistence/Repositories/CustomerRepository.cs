using ImplementingRateLimitingMiddleware.Models;
using ImplementingRateLimitingMiddleware.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImplementingRateLimitingMiddleware.Persistence.Repositories;

public sealed class CustomerRepository(CustomerDbContext dbContext) : ICustomerRepository
{
    public async Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await dbContext.Customers.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<Customer>> GetCustomersAsync(CancellationToken cancellationToken = default) =>
        await dbContext.Customers.AsNoTracking().ToListAsync(cancellationToken);
}
