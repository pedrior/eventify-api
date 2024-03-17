using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Producers.Queries.GetEvents;

internal sealed class GetProducerEventsQueryAuthorizer : IAuthorizer<GetProducerEventsQuery>
{
    public IEnumerable<IRequirement> GetRequirements(GetProducerEventsQuery request)
    {
        yield return new RoleRequirement(Roles.Producer);
    }
}