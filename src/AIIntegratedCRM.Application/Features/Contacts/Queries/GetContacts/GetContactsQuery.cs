using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Application.Features.Contacts.DTOs;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Contacts.Queries.GetContacts;

public record GetContactsQuery : IRequest<Result<PaginatedList<ContactDto>>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? SearchTerm { get; init; }
    public Guid? AccountId { get; init; }
    public string? SortBy { get; init; } = "CreatedAt";
    public bool SortDescending { get; init; } = true;
}
