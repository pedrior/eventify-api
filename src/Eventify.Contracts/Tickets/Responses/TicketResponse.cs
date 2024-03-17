namespace Eventify.Contracts.Tickets.Responses;

public sealed record TicketResponse
{
    public Guid Id { get; init; }

    public Guid EventId { get; init; }

    public string Name { get; init; } = null!;

    public string? Description { get; init; }

    public bool IsSoldOut { get; init; }

    public decimal Price { get; init; }

    public int Quantity { get; init; }

    public int QuantitySold { get; init; }

    public int QuantityPerSale { get; init; }

    public DateTimeOffset? SaleStart { get; init; }

    public DateTimeOffset? SaleEnd { get; init; }

    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset? UpdatedAt { get; init; }
}