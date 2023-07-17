using ImplementingRateLimitingMiddleware.Dtos;
using ImplementingRateLimitingMiddleware.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ImplementingRateLimitingMiddleware.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // GET: api/customers
    [EnableRateLimiting("fixed")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers(CancellationToken cancellationToken)
    {
        var dtos = await _customerService.GetCustomersAsync(cancellationToken);
        return Ok(dtos);
    }

    // GET api/customers/31a7ffcf-d099-4637-bd58-2a87641d1aaf
    [DisableRateLimiting]
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(Guid id, CancellationToken cancellationToken)
    {
        var dto = await _customerService.GetCustomerByIdAsync(id, cancellationToken);

        if (dto is null) 
            return NotFound();

        return Ok(dto);
    }
}
