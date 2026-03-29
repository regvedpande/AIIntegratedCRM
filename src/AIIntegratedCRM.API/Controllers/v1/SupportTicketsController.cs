using AIIntegratedCRM.Application.Features.SupportTickets.Commands.CreateTicket;
using AIIntegratedCRM.Application.Features.SupportTickets.Queries.GetTickets;
using AIIntegratedCRM.Domain.Enums;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIIntegratedCRM.API.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class SupportTicketsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? searchTerm = null,
        [FromQuery] TicketStatus? status = null,
        [FromQuery] TicketPriority? priority = null,
        CancellationToken ct = default)
    {
        var query = new GetTicketsQuery
        {
            PageNumber = pageNumber, PageSize = pageSize, SearchTerm = searchTerm,
            Status = status, Priority = priority
        };
        return HandleResult(await Mediator.Send(query, ct));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTicketCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        if (result.IsSuccess) return Ok(new { id = result.Value });
        return BadRequest(new { error = result.Error });
    }
}
