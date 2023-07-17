using ImplementingRateLimitingMiddleware.Models;
using ImplementingRateLimitingMiddleware.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ImplementingRateLimitingMiddleware.Persistence.Repositories;

public class CustomerRepository: ICustomerRepository
{
    private readonly CustomerDbContext _dbContext;

    public CustomerRepository(CustomerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var keyValues = new object[] { id };
        return await _dbContext.Customers.FindAsync(keyValues, cancellationToken);
    }

    public async Task<IReadOnlyList<Customer>> GetCustomersAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers.AsNoTracking().ToListAsync(cancellationToken);
    }
}
