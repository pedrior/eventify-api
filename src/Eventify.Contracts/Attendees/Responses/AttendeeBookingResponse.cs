namespace Eventify.Contracts.Attendees.Responses;

public sealed record AttendeeBookingResponse
{
    public Guid Id { get; init; }

    public string State { get; init; } = null!;
    
    public decimal TotalPrice { get; init; }
    
    public int TotalQuantity { get; init; }
    
    public DateTimeOffset PlacedAt { get; init; }
    
    public AttendeeBookingTicketResponse Ticket { get; init; } = null!;
    
    public AttendeeBookingEventResponse Event { get; init; } = null!;
}