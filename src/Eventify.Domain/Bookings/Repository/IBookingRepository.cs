using Eventify.Domain.Attendees.ValueObjects;
using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Common.Repository;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Domain.Bookings.Repository;

public interface IBookingRepository : IRepository<Booking, BookingId>
{
    Task<IReadOnlyCollection<Booking>> ListByTicketAsync(TicketId ticketId,
        CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Booking>> ListByEventAsync(EventId eventId,
        CancellationToken cancellationToken = default);
    
    Task<bool> IsOwnerAsync(BookingId bookingId, AttendeeId attendeeId,
        CancellationToken cancellationToken = default);
}