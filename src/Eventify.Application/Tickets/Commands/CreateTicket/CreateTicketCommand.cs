using Eventify.Application.Common.Abstractions.Data;
using Eventify.Contracts.Tickets.Responses;

namespace Eventify.Application.Tickets.Commands.CreateTicket;

public sealed record CreateTicketCommand : ICommand<TicketResponse>, ITransactional
{
    public required Guid EventId { get; init; }

    public required string Name { get; init; }

    public required string? Description { get; init; }

    public required decimal Price { get; init; }

    public required int Quantity { get; init; }

    public required int QuantityPerSale { get; init; }

    public DateTimeOffset? SaleStart { get; init; }

    public DateTimeOffset? SaleEnd { get; init; }
}