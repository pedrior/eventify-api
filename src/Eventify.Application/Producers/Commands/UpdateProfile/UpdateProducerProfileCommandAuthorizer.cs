using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Producers.Commands.UpdateProfile;

internal sealed class UpdateProducerProfileCommandAuthorizer : IAuthorizer<UpdateProducerProfileCommand>
{
    public IEnumerable<IRequirement> GetRequirements(UpdateProducerProfileCommand request)
    {
        yield return new RoleRequirement(Roles.Producer);
    }
}