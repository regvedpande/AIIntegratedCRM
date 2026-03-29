using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Application.Features.SupportTickets.DTOs;
using AIIntegratedCRM.Domain.Enums;
using MediatR;

namespace AIIntegratedCRM.Application.Features.SupportTickets.Queries.GetTickets;

public record GetTicketsQuery : IRequest<Result<PaginatedList<SupportTicketDto>>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 20;
    public string? SearchTerm { get; init; }
    public TicketStatus? Status { get; init; }
    public TicketPriority? Priority { get; init; }
    public Guid? AssignedToUserId { get; init; }
    public string? SortBy { get; init; } = "CreatedAt";
    public bool SortDescending { get; init; } = true;
}
