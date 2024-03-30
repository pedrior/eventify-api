using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.TestUtils.Factories;

public static partial class Factories
{
    public static class Ticket
    {
        public static Domain.Tickets.Ticket CreateTicketValue(
            TicketId? ticketId = null,
            EventId? eventId = null,
            string? name = Constants.Constants.Ticket.Name,
            string? description = Constants.Constants.Ticket.Description,
            Money? price = null,
            Quantity? quantity = null,
            Quantity? quantityPerSale = null,
            DateTimeOffset? saleStart = null,
            DateTimeOffset? saleEnd = null)
        {
            return CreateTicket(
                ticketId,
                eventId,
                name,
                description,
                price,
                quantity,
                quantityPerSale,
                saleStart,
                saleEnd).Value;
        }
        
        public static Result<Domain.Tickets.Ticket> CreateTicket(
            TicketId? ticketId = null,
            EventId? eventId = null,
            string? name = Constants.Constants.Ticket.Name,
            string? description = Constants.Constants.Ticket.Description,
            Money? price = null,
            Quantity? quantity = null,
            Quantity? quantityPerSale = null,
            DateTimeOffset? saleStart = null,
            DateTimeOffset? saleEnd = null)
        {
            return Domain.Tickets.Ticket.Create(
                ticketId ?? Constants.Constants.Ticket.TicketId,
                eventId ?? Constants.Constants.Event.EventId,
                name ?? Constants.Constants.Ticket.Name,
                description: description,
                price ?? Constants.Constants.Ticket.Price,
                quantity ?? Constants.Constants.Ticket.Quantity,
                quantityPerSale ?? Constants.Constants.Ticket.QuantityPerSale,
                saleStart ?? Constants.Constants.Ticket.SaleStart,
                saleEnd ?? Constants.Constants.Ticket.SaleEnd);
        }
    }
}