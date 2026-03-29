using AIIntegratedCRM.Domain.Common;
using AIIntegratedCRM.Domain.Enums;
using AIIntegratedCRM.Domain.Events;

namespace AIIntegratedCRM.Domain.Entities;

public class Opportunity : AuditableEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid? AccountId { get; set; }
    public Guid? PrimaryContactId { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public OpportunityStage Stage { get; set; } = OpportunityStage.Prospecting;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public decimal Probability { get; set; }
    public decimal AIProbability { get; set; }
    public string? AIProbabilityReason { get; set; }
    public DateTime? ExpectedCloseDate { get; set; }
    public DateTime? ActualCloseDate { get; set; }
    public string? Description { get; set; }
    public string? LostReason { get; set; }
    public string? CompetitorInfo { get; set; }
    public string? NextStep { get; set; }
    public string? Tags { get; set; }
    public string? LeadSource { get; set; }

    public Account? Account { get; set; }
    public Contact? PrimaryContact { get; set; }
    public ICollection<Activity> Activities { get; set; } = new List<Activity>();

    public void ChangeStage(OpportunityStage newStage, string? reason = null)
    {
        var oldStage = Stage;
        Stage = newStage;
        if (newStage == OpportunityStage.ClosedWon || newStage == OpportunityStage.ClosedLost)
        {
            ActualCloseDate = DateTime.UtcNow;
            if (newStage == OpportunityStage.ClosedLost) LostReason = reason;
        }
        AddDomainEvent(new OpportunityStageChangedEvent(Id, TenantId, oldStage, newStage));
    }
}
