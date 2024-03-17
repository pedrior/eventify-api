using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Events.Commands.DeleteEvent;

internal sealed class DeleteEventCommandAuthorizer : IAuthorizer<DeleteEventCommand>
{
    public IEnumerable<IRequirement> GetRequirements(DeleteEventCommand command)
    {
        yield return new RoleRequirement(Roles.Producer);
        yield return new EventOwnerRequirement(command.EventId);
    }
}