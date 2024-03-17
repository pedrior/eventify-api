using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Producers.Commands.RemovePicture;

internal sealed class RemoveProducerPictureCommandAuthorizer : IAuthorizer<RemoveProducerPictureCommand>
{
    public IEnumerable<IRequirement> GetRequirements(RemoveProducerPictureCommand request)
    {
        yield return new RoleRequirement(Roles.Producer);
    }
}