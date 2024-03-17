using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Tickets.Commands.RemoveTicket;

internal sealed class RemoveTicketCommandAuthorizer : IAuthorizer<RemoveTicketCommand>
{
    public IEnumerable<IRequirement> GetRequirements(RemoveTicketCommand command)
    {
        yield return new RoleRequirement(Roles.Producer);
        yield return new TicketOwnerRequirement(command.TicketId);
    }
}