using Eventify.Contracts.Attendees.Responses;

namespace Eventify.Application.Attendees.Queries.GetProfile;

public sealed record GetAttendeeProfileQuery : IQuery<AttendeeProfileResponse>;