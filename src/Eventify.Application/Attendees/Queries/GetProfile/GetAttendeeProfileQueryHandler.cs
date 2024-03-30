using Eventify.Contracts.Attendees.Responses;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.Attendees.Queries.GetProfile;

internal sealed class GetAttendeeProfileQueryHandler(
    IUser user,
    IAttendeeRepository attendeeRepository
) : IQueryHandler<GetAttendeeProfileQuery, AttendeeProfileResponse>
{
    public async Task<Result<AttendeeProfileResponse>> Handle(GetAttendeeProfileQuery query,
        CancellationToken cancellationToken)
    {
        var attendee = await attendeeRepository.GetByUserAsync(user.Id, cancellationToken);
        return attendee is not null
            ? attendee.Adapt<AttendeeProfileResponse>()
            : throw new ApplicationException("Attendee not found");
    }
}