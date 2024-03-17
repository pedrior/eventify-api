using Eventify.Domain.Events.Events;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Events.Events;

internal sealed class EventFinishedHandler(ILogger<EventFinishedHandler> logger)
    : IDomainEventHandler<EventFinished>
{
    public Task Handle(EventFinished e, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {EventName} domain event", nameof(EventFinished));
        
        return Task.CompletedTask;
    }
}