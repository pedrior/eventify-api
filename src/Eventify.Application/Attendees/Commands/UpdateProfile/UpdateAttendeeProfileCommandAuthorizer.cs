using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Attendees.Commands.UpdateProfile;

internal sealed class UpdateAttendeeProfileCommandAuthorizer : IAuthorizer<UpdateAttendeeProfileCommand>
{
    public IEnumerable<IRequirement> GetRequirements(UpdateAttendeeProfileCommand request)
    {
        yield return new RoleRequirement(Roles.Attendee);
    }
}