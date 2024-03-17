using Eventify.Application.Tickets.Common.Errors;
using Eventify.Contracts.Tickets.Responses;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Repository;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Application.Tickets.Commands.CreateTicket;

internal sealed class CreateTicketCommandHandler(
    ITicketRepository ticketRepository,
    IEventRepository eventRepository
) : ICommandHandler<CreateTicketCommand, TicketResponse>
{
    public async Task<ErrorOr<TicketResponse>> Handle(CreateTicketCommand command,
        CancellationToken cancellationToken)
    {
        if (!await eventRepository.ExistsAsync(command.EventId, cancellationToken))
        {
            return TicketErrors.EventNotFound(command.EventId);
        }

        var result = Ticket.Create(
            ticketId: TicketId.New(),
            eventId: command.EventId,
            name: command.Name,
            description: command.Description,
            price: new Money(command.Price),
            quantity: new Quantity(command.Quantity),
            quantityPerSale: new Quantity(command.QuantityPerSale),
            saleStart: command.SaleStart,
            saleEnd: command.SaleEnd);

        return await result.ThenAsync(async ticket =>
        {
            await ticketRepository.AddAsync(ticket, cancellationToken);
            return ticket.Adapt<TicketResponse>();
        });
    }
}