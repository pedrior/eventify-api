using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Events.Commands.UpdateDetails;

internal sealed class UpdateEventDetailsCommandAuthorizer : IAuthorizer<UpdateEventDetailsCommand>
{
    public IEnumerable<IRequirement> GetRequirements(UpdateEventDetailsCommand command)
    {
        yield return new RoleRequirement(Roles.Producer);
        yield return new EventOwnerRequirement(command.EventId);
    }
}