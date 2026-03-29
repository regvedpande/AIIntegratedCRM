using AIIntegratedCRM.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AIIntegratedCRM.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess) return Ok(result.Value);
        if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
            return NotFound(new { error = result.Error });
        return BadRequest(new { error = result.Error, errors = result.Errors });
    }

    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess) return NoContent();
        if (result.Error?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
            return NotFound(new { error = result.Error });
        return BadRequest(new { error = result.Error, errors = result.Errors });
    }
}
