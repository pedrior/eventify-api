namespace Eventify.Application.Tickets.Commands.RemoveTicket;

public sealed record RemoveTicketCommand : ICommand<Deleted>
{
    public required Guid TicketId { get; init; }
}