using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Tickets.Commands.CreateTicket;

internal sealed class CreateTicketCommandAuthorizer : IAuthorizer<CreateTicketCommand>
{
    public IEnumerable<IRequirement> GetRequirements(CreateTicketCommand command)
    {
        yield return new RoleRequirement(Roles.Producer);
        yield return new EventOwnerRequirement(command.EventId);
    }
}