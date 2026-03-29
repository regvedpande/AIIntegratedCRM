using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Domain.Enums;
using MediatR;

namespace AIIntegratedCRM.Application.Features.SupportTickets.Commands.CreateTicket;

public record CreateTicketCommand : IRequest<Result<Guid>>
{
    public string Subject { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public TicketPriority Priority { get; init; } = TicketPriority.Medium;
    public Guid? ContactId { get; init; }
    public Guid? AccountId { get; init; }
    public Guid? AssignedToUserId { get; init; }
    public string? Tags { get; init; }
    public string? Category { get; init; }
    public DateTime? DueDate { get; init; }
}
