using Eventify.Domain.Attendees;
using Eventify.Domain.Events;
using Eventify.Domain.Tickets;

namespace Eventify.Domain.Bookings.Services;

public interface IBookingService
{
    Result<Booking> PlaceBooking(
        Event @event,
        Ticket ticket,
        Attendee attendee,
        IEnumerable<Booking> existingTicketBookings);
}