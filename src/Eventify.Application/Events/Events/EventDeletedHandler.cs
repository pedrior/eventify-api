using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Producers.Events;
using Eventify.Domain.Tickets.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Events.Events;

internal sealed class EventDeletedHandler(
    IEventRepository eventRepository,
    ITicketRepository ticketRepository,
    ILogger<EventDeletedHandler> logger
) : IDomainEventHandler<EventDeleted>
{
    public async Task Handle(EventDeleted e, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {EventName} domain event", nameof(EventDeleted));

        await RemoveAllEventTicketsAsync(e.Event, cancellationToken);
        await RemoveEventAsync(e.Event, cancellationToken);
    }

    private async Task RemoveAllEventTicketsAsync(Event @event, CancellationToken cancellationToken)
    {
        foreach (var ticket in await ticketRepository.ListByEventAsync(@event.Id, cancellationToken))
        {
            await ticketRepository.RemoveAsync(ticket, cancellationToken);
        }
    }
    
    private Task RemoveEventAsync(Event @event, CancellationToken cancellationToken) =>
        eventRepository.RemoveAsync(@event, cancellationToken);
}