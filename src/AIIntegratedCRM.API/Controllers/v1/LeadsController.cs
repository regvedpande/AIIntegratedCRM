using AIIntegratedCRM.Application.Features.Leads.Commands.CreateLead;
using AIIntegratedCRM.Application.Features.Leads.Commands.DeleteLead;
using AIIntegratedCRM.Application.Features.Leads.Commands.ScoreLead;
using AIIntegratedCRM.Application.Features.Leads.Commands.UpdateLead;
using AIIntegratedCRM.Application.Features.Leads.Queries.GetLeadById;
using AIIntegratedCRM.Application.Features.Leads.Queries.GetLeads;
using AIIntegratedCRM.Domain.Enums;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIIntegratedCRM.API.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class LeadsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? searchTerm = null,
        [FromQuery] LeadStatus? status = null,
        [FromQuery] LeadSource? source = null,
        [FromQuery] string? sortBy = "CreatedAt",
        [FromQuery] bool sortDescending = true,
        CancellationToken ct = default)
    {
        var query = new GetLeadsQuery
        {
            PageNumber = pageNumber, PageSize = pageSize, SearchTerm = searchTerm,
            Status = status, Source = source, SortBy = sortBy, SortDescending = sortDescending
        };
        return HandleResult(await Mediator.Send(query, ct));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
        => HandleResult(await Mediator.Send(new GetLeadByIdQuery(id), ct));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLeadCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        if (result.IsSuccess) return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { id = result.Value });
        return BadRequest(new { error = result.Error });
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateLeadCommand command, CancellationToken ct)
    {
        if (id != command.Id) return BadRequest(new { error = "ID mismatch." });
        return HandleResult(await Mediator.Send(command, ct));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        => HandleResult(await Mediator.Send(new DeleteLeadCommand(id), ct));

    [HttpPost("{id:guid}/score")]
    public async Task<IActionResult> Score(Guid id, CancellationToken ct)
        => HandleResult(await Mediator.Send(new ScoreLeadCommand(id), ct));
}
