using AIIntegratedCRM.Domain.Enums;

namespace AIIntegratedCRM.Application.Features.Opportunities.DTOs;

public class OpportunityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    public Guid? PrimaryContactId { get; set; }
    public string? PrimaryContactName { get; set; }
    public Guid? AssignedToUserId { get; set; }
    public OpportunityStage Stage { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public decimal Probability { get; set; }
    public decimal AIProbability { get; set; }
    public string? AIProbabilityReason { get; set; }
    public DateTime? ExpectedCloseDate { get; set; }
    public DateTime? ActualCloseDate { get; set; }
    public string? Description { get; set; }
    public string? NextStep { get; set; }
    public string? Tags { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal WeightedAmount => Amount * (AIProbability / 100);
}
