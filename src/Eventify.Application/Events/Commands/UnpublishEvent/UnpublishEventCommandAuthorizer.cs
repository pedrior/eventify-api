using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Events.Commands.UnpublishEvent;

internal sealed class UnpublishEventCommandAuthorizer : IAuthorizer<UnpublishEventCommand>
{
    public IEnumerable<IRequirement> GetRequirements(UnpublishEventCommand command)
    {
        yield return new RoleRequirement(Roles.Producer);
        yield return new EventOwnerRequirement(command.EventId);
    }
}