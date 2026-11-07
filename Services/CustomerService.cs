using ImplementingRateLimitingMiddleware.Dtos;
using ImplementingRateLimitingMiddleware.Interfaces;
using ImplementingRateLimitingMiddleware.Mappers;
using ImplementingRateLimitingMiddleware.Persistence.Interfaces;

namespace ImplementingRateLimitingMiddleware.Services;

public sealed class CustomerService(ICustomerRepository customerRepository) : ICustomerService
{
    public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetCustomerByIdAsync(id, cancellationToken);
        return customer?.ToCustomerDto();
    }

    public async Task<IEnumerable<CustomerDto>> GetCustomersAsync(CancellationToken cancellationToken = default)
    {
        var customers = await customerRepository.GetCustomersAsync(cancellationToken);
        return customers.Select(x => x.ToCustomerDto());
    }
}
