using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Events.Queries.GetEventEditable;

internal sealed class GetEventEditableQueryAuthorizer : IAuthorizer<GetEventEditableQuery>
{
    public IEnumerable<IRequirement> GetRequirements(GetEventEditableQuery query)
    {
        yield return new RoleRequirement(Roles.Producer);
        yield return new EventOwnerRequirement(query.EventId);
    }
}