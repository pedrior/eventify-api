using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Events.Commands.UpdateSlug;

internal sealed class UpdateEventSlugCommandAuthorizer : IAuthorizer<UpdateEventSlugCommand>
{
    public IEnumerable<IRequirement> GetRequirements(UpdateEventSlugCommand command)
    {
        yield return new RoleRequirement(Roles.Producer);
        yield return new EventOwnerRequirement(command.EventId);
    }
}