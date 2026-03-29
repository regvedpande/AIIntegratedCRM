using AIIntegratedCRM.Application.Features.Activities.Commands.CreateActivity;
using AIIntegratedCRM.Application.Features.Activities.Commands.SummarizeMeeting;
using AIIntegratedCRM.Application.Features.Activities.Queries.GetActivities;
using AIIntegratedCRM.Domain.Enums;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIIntegratedCRM.API.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class ActivitiesController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] ActivityType? type = null,
        [FromQuery] Guid? contactId = null,
        [FromQuery] Guid? accountId = null,
        [FromQuery] Guid? opportunityId = null,
        [FromQuery] bool? isCompleted = null,
        CancellationToken ct = default)
    {
        var query = new GetActivitiesQuery
        {
            PageNumber = pageNumber, PageSize = pageSize, Type = type,
            ContactId = contactId, AccountId = accountId, OpportunityId = opportunityId, IsCompleted = isCompleted
        };
        return HandleResult(await Mediator.Send(query, ct));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateActivityCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        if (result.IsSuccess) return Ok(new { id = result.Value });
        return BadRequest(new { error = result.Error });
    }

    [HttpPost("{id:guid}/summarize")]
    public async Task<IActionResult> Summarize(Guid id, [FromBody] SummarizeRequest request, CancellationToken ct)
    {
        var command = new SummarizeMeetingCommand(id, request.Transcript);
        return HandleResult(await Mediator.Send(command, ct));
    }
}

public record SummarizeRequest(string Transcript);
