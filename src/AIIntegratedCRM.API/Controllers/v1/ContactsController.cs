using AIIntegratedCRM.Application.Features.Contacts.Commands.CreateContact;
using AIIntegratedCRM.Application.Features.Contacts.Queries.GetContacts;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIIntegratedCRM.API.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class ContactsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? searchTerm = null,
        [FromQuery] Guid? accountId = null,
        [FromQuery] string? sortBy = "CreatedAt",
        [FromQuery] bool sortDescending = true,
        CancellationToken ct = default)
    {
        var query = new GetContactsQuery
        {
            PageNumber = pageNumber, PageSize = pageSize, SearchTerm = searchTerm,
            AccountId = accountId, SortBy = sortBy, SortDescending = sortDescending
        };
        return HandleResult(await Mediator.Send(query, ct));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateContactCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        if (result.IsSuccess) return Ok(new { id = result.Value });
        return BadRequest(new { error = result.Error });
    }
}
