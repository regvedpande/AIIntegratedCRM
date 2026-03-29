using AIIntegratedCRM.Application.Features.Accounts.Commands.CreateAccount;
using AIIntegratedCRM.Application.Features.Accounts.Queries.GetAccounts;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIIntegratedCRM.API.Controllers.v1;

[ApiVersion("1.0")]
[Authorize]
public class AccountsController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? industry = null,
        [FromQuery] string? sortBy = "CreatedAt",
        [FromQuery] bool sortDescending = true,
        CancellationToken ct = default)
    {
        var query = new GetAccountsQuery
        {
            PageNumber = pageNumber, PageSize = pageSize, SearchTerm = searchTerm,
            Industry = industry, SortBy = sortBy, SortDescending = sortDescending
        };
        return HandleResult(await Mediator.Send(query, ct));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAccountCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        if (result.IsSuccess) return Ok(new { id = result.Value });
        return BadRequest(new { error = result.Error });
    }
}
