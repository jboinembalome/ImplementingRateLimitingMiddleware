using ImplementingRateLimitingMiddleware.Dtos;
using ImplementingRateLimitingMiddleware.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ImplementingRateLimitingMiddleware.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class CustomersController(ICustomerService customerService) : ControllerBase
{
    [EnableRateLimiting("fixed")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<IReadOnlyList<CustomerDto>>> GetCustomers(CancellationToken cancellationToken)
    {
        var dtos = await customerService.GetCustomersAsync(cancellationToken);
        return Ok(dtos);
    }

    [DisableRateLimiting]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(Guid id, CancellationToken cancellationToken)
    {
        var dto = await customerService.GetCustomerByIdAsync(id, cancellationToken);

        if (dto is null)
            return NotFound();

        return Ok(dto);
    }
}
