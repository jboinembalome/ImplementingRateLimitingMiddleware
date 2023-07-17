using ImplementingRateLimitingMiddleware.Models;

namespace ImplementingRateLimitingMiddleware.Persistence.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Customer>> GetCustomersAsync(CancellationToken cancellationToken = default);

}
