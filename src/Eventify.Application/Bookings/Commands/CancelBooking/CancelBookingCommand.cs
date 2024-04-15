using Eventify.Application.Common.Abstractions.Requests;

namespace Eventify.Application.Bookings.Commands.CancelBooking;

public sealed record CancelBookingCommand : ICommand<Success>
{
    public required Guid BookingId { get; init; }
}