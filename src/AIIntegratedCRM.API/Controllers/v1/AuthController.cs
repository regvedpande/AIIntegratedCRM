using AIIntegratedCRM.Application.Features.Auth.Commands.Login;
using AIIntegratedCRM.Application.Features.Auth.Commands.Register;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace AIIntegratedCRM.API.Controllers.v1;

[ApiVersion("1.0")]
public class AuthController : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        if (result.IsSuccess) return Ok(new { userId = result.Value, message = "Registration successful." });
        return BadRequest(new { error = result.Error });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        return HandleResult(result);
    }
}
