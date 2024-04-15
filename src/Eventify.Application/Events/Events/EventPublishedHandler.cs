using Eventify.Domain.Events.Events;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Events.Events;

internal sealed class EventPublishedHandler(ILogger<EventPublishedHandler> logger)
    : IDomainEventHandler<EventPublished>
{
    public Task Handle(EventPublished e, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {EventName} domain event", nameof(EventPublished));

        return Task.CompletedTask;
    }
}