using Eventify.Domain.Common.Entities;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Tickets.Errors;
using Eventify.Domain.Tickets.Events;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Domain.Tickets;

public sealed class Ticket : Entity<TicketId>, IAggregateRoot, IAuditable, ISoftDelete
{
    private Ticket(TicketId id) : base(id)
    {
    }

    public required EventId EventId { get; init; }

    public string Name { get; private set; } = default!;

    public string? Description { get; private set; }

    public Money Price { get; private set; } = default!;

    public Quantity Quantity { get; private set; } = default!;

    public Quantity QuantitySold { get; private set; } = default!;

    public Quantity QuantityPerSale { get; private set; } = default!;

    public bool IsDeleted { get; set; }

    public DateTimeOffset? SaleStart { get; private set; }

    public DateTimeOffset? SaleEnd { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public bool IsSold => QuantitySold > Quantity.Zero;

    public bool IsSoldOut => Quantity - QuantitySold < QuantityPerSale;

    public static ErrorOr<Ticket> Create(
        TicketId ticketId,
        EventId eventId,
        string name,
        string? description,
        Money price,
        Quantity quantity,
        Quantity quantityPerSale,
        DateTimeOffset? saleStart,
        DateTimeOffset? saleEnd)
    {
        if (quantityPerSale == Quantity.Zero)
        {
            return TicketErrors.QuantityPerSaleMustBeAtLeastOne;
        }

        var ticket = new Ticket(ticketId)
        {
            EventId = eventId,
            Name = name,
            Description = description,
            Price = price,
            Quantity = quantity,
            QuantitySold = Quantity.Zero,
            QuantityPerSale = quantityPerSale,
            SaleStart = saleStart,
            SaleEnd = saleEnd
        };

        ticket.RaiseDomainEvent(new TicketCreated(ticket));

        return ticket;
    }

    public ErrorOr<Updated> Update(
        string name,
        Money price,
        Quantity quantity,
        Quantity quantityPerSale,
        DateTimeOffset? saleStart = null,
        DateTimeOffset? saleEnd = null,
        string? description = null)
    {
        if (quantityPerSale == Quantity.Zero)
        {
            return TicketErrors.QuantityPerSaleMustBeAtLeastOne;
        }

        if (quantity < QuantitySold)
        {
            return TicketErrors.QuantityMustBeGreaterThanQuantitySold(QuantitySold);
        }

        Name = name;
        Description = description;
        Price = price;
        Quantity = quantity;
        QuantityPerSale = quantityPerSale;
        SaleStart = saleStart;
        SaleEnd = saleEnd;

        return Result.Updated;
    }

    public bool IsAvailable() => !IsSoldOut && IsInSalePeriod();

    public ErrorOr<Success> Sell()
    {
        if (IsSoldOut)
        {
            return TicketErrors.SoldOut;
        }

        if (!IsInSalePeriod())
        {
            return TicketErrors.NotInSalePeriod(SaleStart, SaleEnd);
        }

        QuantitySold += QuantityPerSale;

        return Result.Success;
    }

    public ErrorOr<Success> CancelSale()
    {
        if (QuantitySold == Quantity.Zero)
        {
            return TicketErrors.NoSaleToCancel;
        }

        QuantitySold -= QuantityPerSale;

        return Result.Success;
    }

    private bool IsInSalePeriod() => (SaleStart, SaleEnd) switch
    {
        (null, null) => true,
        (null, { } saleEnd) => DateTimeOffset.UtcNow < saleEnd,
        ({ } saleStart, null) => saleStart < DateTimeOffset.UtcNow,
        ({ } saleStart, { } saleEnd) => saleStart < DateTimeOffset.UtcNow && DateTimeOffset.UtcNow < saleEnd
    };
}