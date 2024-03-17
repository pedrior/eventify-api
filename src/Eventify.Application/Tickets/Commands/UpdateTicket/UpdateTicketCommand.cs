namespace Eventify.Application.Tickets.Commands.UpdateTicket;

public sealed record UpdateTicketCommand : ICommand<Updated>
{
    public required Guid TicketId { get; init; }

    public required string Name { get; init; }

    public required string? Description { get; init; }

    public required decimal Price { get; init; }

    public required int Quantity { get; init; }

    public required int QuantityPerSale { get; init; }

    public DateTimeOffset? SaleStart { get; init; }

    public DateTimeOffset? SaleEnd { get; init; }
}