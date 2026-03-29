using AIIntegratedCRM.Domain.Common;
using AIIntegratedCRM.Domain.Enums;

namespace AIIntegratedCRM.Domain.Entities;

public class SupportTicket : AuditableEntity
{
    public string Subject { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketStatus Status { get; set; } = TicketStatus.Open;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public Guid? ContactId { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public string? Resolution { get; set; }
    public string? Tags { get; set; }
    public string? Category { get; set; }
    public int? SatisfactionScore { get; set; }

    public Contact? Contact { get; set; }
    public Account? Account { get; set; }
    public ICollection<TicketComment> Comments { get; set; } = new List<TicketComment>();

    public void Resolve(string resolution)
    {
        Status = TicketStatus.Resolved;
        Resolution = resolution;
        ResolvedAt = DateTime.UtcNow;
    }
}
