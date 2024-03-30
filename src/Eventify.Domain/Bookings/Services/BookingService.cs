using Eventify.Domain.Attendees;
using Eventify.Domain.Bookings.Errors;
using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Events;
using Eventify.Domain.Tickets;

namespace Eventify.Domain.Bookings.Services;

public sealed class BookingService : IBookingService
{
    public Result<Booking> PlaceBooking(
        Event @event,
        Ticket ticket,
        Attendee attendee,
        IEnumerable<Booking> existingTicketBookings)
    {
        if (!@event.IsPublished)
        {
            return BookingErrors.RequiredPublishedEvent(@event.Id);
        }

        if (!ticket.IsAvailable())
        {
            return BookingErrors.UnavailableTicket(ticket.Id);
        }

        var activeBooking = existingTicketBookings.SingleOrDefault(b => b.State.IsActive());
        if (activeBooking is not null)
        {
            return BookingErrors.MultipleActiveBooking(activeBooking.Id, ticket.Id);
        }

        return Booking.Place(
            bookingId: BookingId.New(),
            attendeeId: attendee.Id,
            ticketId: ticket.Id,
            eventId: @event.Id,
            totalPrice: ticket.Price * ticket.QuantityPerSale,
            totalQuantity: ticket.QuantityPerSale);
    }
}