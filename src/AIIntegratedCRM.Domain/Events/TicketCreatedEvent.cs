using MediatR;

namespace AIIntegratedCRM.Domain.Events;

public record TicketCreatedEvent(Guid TicketId, Guid TenantId, string Subject) : INotification;
