namespace Eventify.Contracts.Bookings.Responses;

public sealed record BookingEventResponse
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;
    
    public string Location { get; init; } = null!;
    
    public DateTimeOffset Start { get; init; }
    
    public DateTimeOffset End { get; init; }
}