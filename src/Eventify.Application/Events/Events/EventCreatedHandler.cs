using Eventify.Domain.Events;
using Eventify.Domain.Events.Events;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Producers.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Events.Events;

internal sealed class EventCreatedHandler(
    IProducerRepository producerRepository,
    ILogger<EventCreatedHandler> logger
) : IDomainEventHandler<EventCreated>
{
    public Task Handle(EventCreated e, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {EventName} domain event", nameof(EventCreated));
        
        return AddEventToProducerAsync(e.Event, e.Event.ProducerId, cancellationToken);
    }

    private async Task AddEventToProducerAsync(Event @event, ProducerId producerId,
        CancellationToken cancellationToken)
    {
        var producer = await producerRepository.GetAsync(producerId, cancellationToken);
        if (producer is null)
        {
            throw new ApplicationException($"Producer {@event.ProducerId} not found");
        }

        var result = await producer.AddEvent(@event)
            .ThenAsync(() => producerRepository.UpdateAsync(producer, cancellationToken));

        result.EnsureSuccess();
    }
}