using ImplementingRateLimitingMiddleware.Dtos;
using ImplementingRateLimitingMiddleware.Interfaces;
using ImplementingRateLimitingMiddleware.Mappers;
using ImplementingRateLimitingMiddleware.Persistence.Interfaces;

namespace ImplementingRateLimitingMiddleware.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;       
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetCustomerByIdAsync(id, cancellationToken);
        return customer?.ToCustomerDto();
    }

    public async Task<IEnumerable<CustomerDto>> GetCustomersAsync(CancellationToken cancellationToken = default)
    {
        var customers = await _customerRepository.GetCustomersAsync(cancellationToken);
        return customers.Select(x => x.ToCustomerDto());
    }
}
