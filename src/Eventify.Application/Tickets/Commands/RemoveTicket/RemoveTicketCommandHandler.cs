using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Application.Tickets.Common.Errors;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Tickets.Repository;

namespace Eventify.Application.Tickets.Commands.RemoveTicket;

internal sealed class RemoveTicketCommandHandler(
    IEventRepository eventRepository,
    ITicketRepository ticketRepository)
    : ICommandHandler<RemoveTicketCommand, Success>
{
    public async Task<Result<Success>> Handle(RemoveTicketCommand command,
        CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetAsync(command.TicketId, cancellationToken);
        if (ticket is null)
        {
            return TicketErrors.NotFound(command.TicketId);
        }

        var @event = await eventRepository.GetAsync(ticket.EventId, cancellationToken);
        if (@event is null)
        {
            throw new ApplicationException($"Event {ticket.EventId} not found");
        }

        return await @event.RemoveTicket(ticket)
            .ThenAsync(() => eventRepository.UpdateAsync(@event, cancellationToken));
    }
}