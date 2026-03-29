using AIIntegratedCRM.Application.Features.AI.Commands.GenerateEmail;
using AIIntegratedCRM.Application.Features.AI.Queries.NaturalLanguageQuery;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIIntegratedCRM.API.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class AIController : ApiControllerBase
{
    [HttpPost("query")]
    public async Task<IActionResult> NaturalLanguageQuery([FromBody] NLQueryRequest request, CancellationToken ct)
    {
        var command = new NLQueryCommand(request.Query);
        return HandleResult(await Mediator.Send(command, ct));
    }

    [HttpPost("generate-email")]
    public async Task<IActionResult> GenerateEmail([FromBody] GenerateEmailCommand command, CancellationToken ct)
        => HandleResult(await Mediator.Send(command, ct));
}

public record NLQueryRequest(string Query);
