using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Producers.Queries.GetProfile;

internal sealed class GetProducerProfileQueryAuthorizer : IAuthorizer<GetProducerProfileQuery>
{
    public IEnumerable<IRequirement> GetRequirements(GetProducerProfileQuery _)
    {
        yield return new RoleRequirement(Roles.Producer);
    }
}