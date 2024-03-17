namespace Eventify.Contracts.Tickets.Requests;

public sealed record CreateTicketRequest
{
    public Guid EventId { get; init; }
    
    public string Name { get; init; } = null!;

    public string? Description { get; init; }

    public decimal Price { get; init; }

    public int Quantity { get; init; }
    
    public int QuantityPerSale { get; init; }
    
    public DateTimeOffset? SaleStart { get; init; }

    public DateTimeOffset? SaleEnd { get; init; }
}