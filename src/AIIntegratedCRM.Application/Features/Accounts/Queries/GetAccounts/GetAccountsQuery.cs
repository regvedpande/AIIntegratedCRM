using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Application.Features.Accounts.DTOs;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Accounts.Queries.GetAccounts;

public record GetAccountsQuery : IRequest<Result<PaginatedList<AccountDto>>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? SearchTerm { get; init; }
    public string? Industry { get; init; }
    public string? SortBy { get; init; } = "CreatedAt";
    public bool SortDescending { get; init; } = true;
}
