using Eventify.Domain.Attendees.ValueObjects;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;
using Eventify.Infrastructure.Common.Persistence;

namespace Eventify.Infrastructure.Bookings.Data;

internal sealed class BookingRepository(DataContext context)
    : Repository<Booking, BookingId, DataContext>(context), IBookingRepository
{
    public async Task<IReadOnlyCollection<Booking>> ListByTicketAsync(TicketId ticketId,
        CancellationToken cancellationToken = default)
    {
        return (await Set.AsNoTracking()
                .Where(b => b.TicketId == ticketId)
                .ToListAsync(cancellationToken))
            .AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Booking>> ListByEventAsync(EventId eventId,
        CancellationToken cancellationToken = default)
    {
        return (await Set.AsNoTracking()
                .Where(b => b.EventId == eventId)
                .ToListAsync(cancellationToken))
            .AsReadOnly();
    }

    public Task<bool> IsOwnerAsync(BookingId bookingId, AttendeeId attendeeId,
        CancellationToken cancellationToken = default)
    {
        return Set.AnyAsync(b => b.Id == bookingId && b.AttendeeId == attendeeId, cancellationToken);
    }
}