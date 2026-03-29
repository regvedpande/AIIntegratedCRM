using AIIntegratedCRM.Application.Features.Opportunities.Commands.ChangeStage;
using AIIntegratedCRM.Application.Features.Opportunities.Commands.CreateOpportunity;
using AIIntegratedCRM.Application.Features.Opportunities.Queries.GetOpportunities;
using AIIntegratedCRM.Domain.Enums;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIIntegratedCRM.API.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class OpportunitiesController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? searchTerm = null,
        [FromQuery] OpportunityStage? stage = null,
        [FromQuery] Guid? accountId = null,
        [FromQuery] string? sortBy = "CreatedAt",
        [FromQuery] bool sortDescending = true,
        CancellationToken ct = default)
    {
        var query = new GetOpportunitiesQuery
        {
            PageNumber = pageNumber, PageSize = pageSize, SearchTerm = searchTerm,
            Stage = stage, AccountId = accountId, SortBy = sortBy, SortDescending = sortDescending
        };
        return HandleResult(await Mediator.Send(query, ct));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOpportunityCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        if (result.IsSuccess) return Ok(new { id = result.Value });
        return BadRequest(new { error = result.Error });
    }

    [HttpPatch("{id:guid}/stage")]
    public async Task<IActionResult> ChangeStage(Guid id, [FromBody] ChangeStageRequest request, CancellationToken ct)
    {
        var command = new ChangeOpportunityStageCommand(id, request.NewStage, request.Reason);
        return HandleResult(await Mediator.Send(command, ct));
    }
}

public record ChangeStageRequest(OpportunityStage NewStage, string? Reason = null);
