using AIIntegratedCRM.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AIIntegratedCRM.Infrastructure.Services;

public class EventBusService : IEventBus
{
    private readonly ILogger<EventBusService> _logger;

    public EventBusService(ILogger<EventBusService> logger)
    {
        _logger = logger;
    }

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class
    {
        try
        {
            var eventName = typeof(T).Name;
            var payload = JsonSerializer.Serialize(@event);
            _logger.LogInformation("[EventBus] Publishing event: {EventName} | Payload: {Payload}", eventName, payload);
            // In production, integrate with MassTransit + RabbitMQ here
            // await _busControl.Publish(@event, cancellationToken);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing event {EventType}", typeof(T).Name);
        }
    }
}
