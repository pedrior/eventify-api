using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Producers.Commands.UploadPicture;

internal sealed class UploadProducerPictureCommandAuthorizer : IAuthorizer<UploadProducerPictureCommand>
{
    public IEnumerable<IRequirement> GetRequirements(UploadProducerPictureCommand request)
    {
        yield return new RoleRequirement(Roles.Producer);
    }
}