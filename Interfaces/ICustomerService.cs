using ImplementingRateLimitingMiddleware.Dtos;

namespace ImplementingRateLimitingMiddleware.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<CustomerDto>> GetCustomersAsync(CancellationToken cancellationToken = default);
}
