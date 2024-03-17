using Eventify.Contracts.Bookings.Responses;

namespace Eventify.Application.Bookings.Queries.GetBooking;

public sealed record GetBookingQuery : IQuery<BookingResponse>
{
    public required Guid BookingId { get; init; }
}