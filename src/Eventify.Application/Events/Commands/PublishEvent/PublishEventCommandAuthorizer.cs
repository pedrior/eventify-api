using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Events.Commands.PublishEvent;

internal sealed class PublishEventCommandAuthorizer : IAuthorizer<PublishEventCommand>
{
    public IEnumerable<IRequirement> GetRequirements(PublishEventCommand command)
    {
        yield return new RoleRequirement(Roles.Producer);
        yield return new EventOwnerRequirement(command.EventId);
    }
}