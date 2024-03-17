using Eventify.Application.Common.Abstractions.Data;

namespace Eventify.Application.Attendees.Commands.UpdateProfile;

public sealed record UpdateAttendeeProfileCommand : ICommand<Updated>, ITransactional
{
    public required string GivenName { get; init; }

    public required string FamilyName { get; init; }

    public string? PhoneNumber { get; init; }

    public DateOnly? BirthDate { get; init; }
}