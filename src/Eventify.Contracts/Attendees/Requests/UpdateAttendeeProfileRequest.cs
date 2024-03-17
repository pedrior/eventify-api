namespace Eventify.Contracts.Attendees.Requests;

public sealed record UpdateAttendeeProfileRequest
{
    public string GivenName { get; init; } = null!;

    public string FamilyName { get; init; } = null!;
    
    public string? PhoneNumber { get; init; }
    
    public DateOnly? BirthDate { get; init; }
}