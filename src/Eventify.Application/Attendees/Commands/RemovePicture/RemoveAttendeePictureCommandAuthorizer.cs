using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Attendees.Commands.RemovePicture;

internal sealed class RemoveAttendeePictureCommandAuthorizer : IAuthorizer<RemoveAttendeePictureCommand>
{
    public IEnumerable<IRequirement> GetRequirements(RemoveAttendeePictureCommand request)
    {
        yield return new RoleRequirement(Roles.Attendee);
    }
}