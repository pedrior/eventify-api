using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Tickets.Queries.GetTicket;

internal sealed class GetTicketQueryAuthorizer : IAuthorizer<GetTicketQuery>
{
    public IEnumerable<IRequirement> GetRequirements(GetTicketQuery query)
    {
        yield return new RoleRequirement(Roles.Producer);
        yield return new TicketOwnerRequirement(query.TicketId);
    }
}