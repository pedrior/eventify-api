using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Events.Commands.UpdateLocation;

internal sealed class UpdateEventLocationCommandAuthorizer : IAuthorizer<UpdateEventLocationCommand>
{
    public IEnumerable<IRequirement> GetRequirements(UpdateEventLocationCommand command)
    {
        yield return new RoleRequirement(Roles.Producer);
        yield return new EventOwnerRequirement(command.EventId);
    }
}