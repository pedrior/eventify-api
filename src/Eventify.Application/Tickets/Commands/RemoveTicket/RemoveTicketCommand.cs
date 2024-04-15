namespace Eventify.Application.Tickets.Commands.RemoveTicket;

public sealed record RemoveTicketCommand : ICommand<Success>
{
    public required Guid TicketId { get; init; }
}