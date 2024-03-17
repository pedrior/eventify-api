namespace Eventify.Contracts.Bookings.Requests;

public sealed record CreateBookingRequest
{
    public Guid TicketId { get; init; }
}