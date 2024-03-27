namespace Eventify.Contracts.Bookings.Responses;

public sealed record BookingResponse
{
    public Guid Id { get; init; }

    public decimal TotalPrice { get; init; }

    public int TotalQuantity { get; init; }

    public string State { get; init; } = null!;

    public DateTimeOffset PlacedAt { get; init; }

    public DateTimeOffset? PaidAt { get; init; }

    public DateTimeOffset? ConfirmedAt { get; init; }

    public DateTimeOffset? CancelledAt { get; init; }
    
    public string? CancellationReason { get; init; }
    
    public BookingEventResponse Event { get; init; } = null!;

    public BookingTicketResponse Ticket { get; init; } = null!;
}