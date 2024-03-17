using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Attendees.Commands.UploadPicture;

internal sealed class UploadAttendeePictureCommandAuthorizer : IAuthorizer<UploadAttendeePictureCommand>
{
    public IEnumerable<IRequirement> GetRequirements(UploadAttendeePictureCommand request)
    {
        yield return new RoleRequirement(Roles.Attendee);
    }
}