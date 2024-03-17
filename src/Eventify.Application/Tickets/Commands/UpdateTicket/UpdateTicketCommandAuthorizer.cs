using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Tickets.Commands.UpdateTicket;

internal sealed class UpdateTicketCommandAuthorizer : IAuthorizer<UpdateTicketCommand>
{
    public IEnumerable<IRequirement> GetRequirements(UpdateTicketCommand command)
    {
        yield return new RoleRequirement(Roles.Producer);
        yield return new TicketOwnerRequirement(command.TicketId);
    }
}