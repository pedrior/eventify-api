using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Events.Commands.CreateEvent;

internal sealed class CreateEventCommandAuthorizer : IAuthorizer<CreateEventCommand>
{
    public IEnumerable<IRequirement> GetRequirements(CreateEventCommand request)
    {
        yield return new RoleRequirement(Roles.Producer);
    }
}