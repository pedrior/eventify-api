using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Producers.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.TestUtils.Factories;

public static partial class Factories
{
    public static class Event
    {
        public static Domain.Events.Event CreateEvent(
            EventId? eventId = null,
            ProducerId? producerId = null,
            EventDetails? details = null,
            EventLocation? location = null,
            Period? period = null,
            Uri? posterUrl = null,
            int tickets = 0)
        {
            var @event = Domain.Events.Event.Create(
                eventId ?? Constants.Constants.Event.EventId,
                producerId ?? Constants.Constants.Producer.ProducerId,
                details ?? Constants.Constants.Event.Details,
                location ?? Constants.Constants.Event.Location,
                period ?? Constants.Constants.Event.Period);

            if (posterUrl is not null)
            {
                @event.SetPoster(posterUrl);
            }
            
            for (var i = 0; i < tickets; i++)
            {
                @event.AddTicket(Ticket.CreateTicketValue(ticketId: TicketId.New()));
            }

            return @event;
        }
    }
}