namespace Eventify.Contracts.Attendees.Responses;

public sealed record AttendeeBookingTicketResponse
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;
    
    public decimal Price { get; init; }
}