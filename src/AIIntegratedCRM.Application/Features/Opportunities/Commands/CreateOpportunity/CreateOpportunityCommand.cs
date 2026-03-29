using AIIntegratedCRM.Application.Common.Models;
using AIIntegratedCRM.Domain.Enums;
using MediatR;

namespace AIIntegratedCRM.Application.Features.Opportunities.Commands.CreateOpportunity;

public record CreateOpportunityCommand : IRequest<Result<Guid>>
{
    public string Name { get; init; } = string.Empty;
    public Guid? AccountId { get; init; }
    public Guid? PrimaryContactId { get; init; }
    public Guid? AssignedToUserId { get; init; }
    public OpportunityStage Stage { get; init; } = OpportunityStage.Prospecting;
    public decimal Amount { get; init; }
    public string Currency { get; init; } = "USD";
    public decimal Probability { get; init; } = 10;
    public DateTime? ExpectedCloseDate { get; init; }
    public string? Description { get; init; }
    public string? NextStep { get; init; }
    public string? Tags { get; init; }
    public string? LeadSource { get; init; }
}
