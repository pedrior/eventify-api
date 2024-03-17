using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Attendees.Queries.GetBookings;

internal sealed class GetAttendeeBookingsQueryAuthorizer : IAuthorizer<GetAttendeeBookingsQuery>
{
    public IEnumerable<IRequirement> GetRequirements(GetAttendeeBookingsQuery request)
    {
        yield return new RoleRequirement(Roles.Attendee);
    }
}