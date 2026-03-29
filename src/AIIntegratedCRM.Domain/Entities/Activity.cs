using AIIntegratedCRM.Domain.Common;
using AIIntegratedCRM.Domain.Enums;

namespace AIIntegratedCRM.Domain.Entities;

public class Activity : AuditableEntity
{
    public ActivityType Type { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int? DurationMinutes { get; set; }
    public Guid? ContactId { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? OpportunityId { get; set; }
    public Guid? LeadId { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime? CompletedAt { get; set; }
    public string? Outcome { get; set; }
    public string? AISummary { get; set; }
    public string? Location { get; set; }
    public string? MeetingUrl { get; set; }
    public string? TranscriptText { get; set; }
    public string? Tags { get; set; }
    public string? Direction { get; set; }

    public Contact? Contact { get; set; }
    public Account? Account { get; set; }
    public Opportunity? Opportunity { get; set; }

    public void Complete(string? outcome = null)
    {
        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
        Outcome = outcome;
    }
}
