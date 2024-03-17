using Eventify.Domain.Attendees.ValueObjects;
using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.TestUtils.Factories;

public static partial class Factories
{
    public static class Booking
    {
        public static Domain.Bookings.Booking CreateBooking(
            BookingId? bookingId = null,
            AttendeeId? attendeeId = null,
            EventId? eventId = null,
            TicketId? ticketId = null,
            Money? totalPrice = null,
            Quantity? totalQuantity = null)
        {
            return Domain.Bookings.Booking.Place(
                bookingId ?? Constants.Constants.Booking.BookingId,
                ticketId ?? Constants.Constants.Ticket.TicketId,
                eventId ?? Constants.Constants.Event.EventId,
                attendeeId ?? Constants.Constants.Attendee.AttendeeId,
                totalPrice ?? Constants.Constants.Booking.TotalPrice,
                totalQuantity ?? Constants.Constants.Booking.TotalQuantity);
        }
    }
}