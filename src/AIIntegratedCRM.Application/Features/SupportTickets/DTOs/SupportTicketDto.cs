using AIIntegratedCRM.Domain.Enums;

namespace AIIntegratedCRM.Application.Features.SupportTickets.DTOs;

public class SupportTicketDto
{
    public Guid Id { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
    public Guid? ContactId { get; set; }
    public string? ContactName { get; set; }
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public string? Resolution { get; set; }
    public string? Tags { get; set; }
    public string? Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<TicketCommentDto> Comments { get; set; } = new();
}

public class TicketCommentDto
{
    public Guid Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string AuthorName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsInternal { get; set; }
}
