using Eventify.Application.Tickets.Common.Errors;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Tickets.Repository;

namespace Eventify.Application.Tickets.Commands.UpdateTicket;

internal sealed class UpdateTicketCommandHandler(ITicketRepository ticketRepository) 
    : ICommandHandler<UpdateTicketCommand, Updated>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateTicketCommand command,
        CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetAsync(command.TicketId, cancellationToken);
        if (ticket is null)
        {
            return TicketErrors.NotFound(command.TicketId);
        }

        var result = ticket.Update(
            name: command.Name,
            description: command.Description,
            price: new Money(command.Price),
            quantity: new Quantity(command.Quantity),
            quantityPerSale: new Quantity(command.QuantityPerSale),
            saleStart: command.SaleStart,
            saleEnd: command.SaleEnd);

        return await result.ThenAsync(_ => ticketRepository.UpdateAsync(ticket, cancellationToken));
    }
}