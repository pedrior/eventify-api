using Eventify.Domain.Events.Repository;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Events;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Tickets.Events;

internal sealed class TicketCreatedHandler(
    IEventRepository eventRepository,
    ILogger<TicketCreatedHandler> logger
) : IDomainEventHandler<TicketCreated>
{
    public Task Handle(TicketCreated e, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {EventName} domain event", nameof(TicketCreated));

        return AddTicketToEventAsync(e.Ticket, cancellationToken);
    }

    private async Task AddTicketToEventAsync(Ticket ticket, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetAsync(ticket.EventId, cancellationToken);
        if (@event is null)
        {
            throw new ApplicationException($"Event {ticket.EventId} not found");
        }

        var result = await @event.AddTicket(ticket)
            .ThenAsync(_ => eventRepository.UpdateAsync(@event, cancellationToken));

        if (result.IsError)
        {
            throw new ApplicationException($"Error adding ticket {ticket.Id} to event {ticket.EventId}: " +
                                           $"{result.FirstError.Description}");
        }
    }
}