using Eventify.Application.Tickets.Common.Errors;
using Eventify.Contracts.Tickets.Responses;
using Eventify.Domain.Tickets.Repository;

namespace Eventify.Application.Tickets.Queries.GetTicket;

internal sealed class GetTicketQueryHandler(ITicketRepository ticketRepository) 
    : IQueryHandler<GetTicketQuery, TicketResponse>
{
    public async Task<ErrorOr<TicketResponse>> Handle(GetTicketQuery query,
        CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetAsync(query.TicketId, cancellationToken);
        return ticket is null 
            ? TicketErrors.NotFound(query.TicketId) 
            : ticket.Adapt<TicketResponse>();
    }
}