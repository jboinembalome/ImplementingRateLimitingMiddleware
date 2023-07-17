using ImplementingRateLimitingMiddleware.Dtos;
using ImplementingRateLimitingMiddleware.Models;

namespace ImplementingRateLimitingMiddleware.Mappers;

public static class CustomerMapperExtension
{
    public static CustomerDto ToCustomerDto(this Customer customer) 
        => new(customer.Id, customer.FirstName, customer.LastName, customer.Email);
}
