namespace Eventify.Contracts.Events.Responses;

public sealed record EventTicketResponse
{
    public Guid Id { get; init; }

    public string Name { get; init; } = null!;

    public string? Description { get; init; }

    public bool IsSoldOut { get; init; }

    public decimal Price { get; init; }

    public int Quantity { get; init; }

    public int QuantitySold { get; init; }

    public int QuantityPerSale { get; init; }

    public DateTimeOffset? SaleStart { get; init; }

    public DateTimeOffset? SaleEnd { get; init; }
}