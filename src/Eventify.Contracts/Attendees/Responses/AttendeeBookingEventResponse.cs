namespace Eventify.Contracts.Attendees.Responses;

public sealed record AttendeeBookingEventResponse
{
    public Guid Id { get; init; }
    
    public string Name { get; init; } = null!;
    
    public DateTimeOffset Start { get; init; }
    
    public DateTimeOffset End { get; init; }

    public string Location { get; init; } = null!;
    
    public string?PosterUrl { get; init; }
}