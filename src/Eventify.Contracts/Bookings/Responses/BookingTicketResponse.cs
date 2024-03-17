namespace Eventify.Contracts.Bookings.Responses;

public sealed record BookingTicketResponse
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;
    
    public decimal Price { get; init; }
}