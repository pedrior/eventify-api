using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Events.Commands.UpdatePeriod;

internal sealed class UpdateEventPeriodCommandAuthorizer : IAuthorizer<UpdateEventPeriodCommand>
{
    public IEnumerable<IRequirement> GetRequirements(UpdateEventPeriodCommand command)
    {
        yield return new RoleRequirement(Roles.Producer);
        yield return new EventOwnerRequirement(command.EventId);
    }
}