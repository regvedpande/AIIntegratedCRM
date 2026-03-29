using AIIntegratedCRM.Domain.Enums;
using MediatR;

namespace AIIntegratedCRM.Domain.Events;

public record OpportunityStageChangedEvent(
    Guid OpportunityId,
    Guid TenantId,
    OpportunityStage OldStage,
    OpportunityStage NewStage) : INotification;
