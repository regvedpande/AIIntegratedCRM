using MediatR;

namespace AIIntegratedCRM.Domain.Events;

public record LeadCreatedEvent(Guid LeadId, Guid TenantId, string Email) : INotification;
