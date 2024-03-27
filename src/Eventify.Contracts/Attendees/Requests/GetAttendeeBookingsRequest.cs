namespace Eventify.Contracts.Attendees.Requests;

public sealed record GetAttendeeBookingsRequest
{
    public int Page { get; init; } = 1;
    
    public int Limit { get; init; } = 10;
}