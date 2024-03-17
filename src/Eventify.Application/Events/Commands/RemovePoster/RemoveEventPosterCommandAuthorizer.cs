using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Events.Commands.RemovePoster;

internal sealed class RemoveEventPosterCommandAuthorizer : IAuthorizer<RemoveEventPosterCommand>
{
    public IEnumerable<IRequirement> GetRequirements(RemoveEventPosterCommand command)
    {
        yield return new RoleRequirement(Roles.Producer);
        yield return new EventOwnerRequirement(command.EventId);
    }
}