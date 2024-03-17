using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Attendees.Queries.GetProfile;

internal sealed class GetAttendeeProfileQueryAuthorizer : IAuthorizer<GetAttendeeProfileQuery>
{
    public IEnumerable<IRequirement> GetRequirements(GetAttendeeProfileQuery request)
    {
        yield return new RoleRequirement(Roles.Attendee);
    }
}