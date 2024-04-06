using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Domain.Tickets.Errors;

internal static class TicketErrors
{
    public static readonly Error SoldOut = Error.Conflict(
        "The ticket is sold out",
        code: "ticket.sold_out");

    public static readonly Error NoSaleToCancel = Error.Conflict(
        "There is no sale to cancel",
        code: "ticket.no_sale_to_cancel");

    public static readonly Error QuantityPerSaleMustBeAtLeastOne = Error.Failure(
        "The quantity per sale must be equal to or greater than one",
        code: "ticket.quantity_per_sale_must_be_at_least_one");

    public static Error NotInSalePeriod(DateTimeOffset? saleStart, DateTimeOffset? saleEnd) => Error.Conflict(
        "The ticket is not in sale period",
        code: "ticket.not_in_sale_period",
        details: new Dictionary<string, object?>
        {
            ["sale_start"] = saleStart?.ToString("O") ?? "null",
            ["sale_end"] = saleEnd?.ToString("O") ?? "null"
        }.ToFrozenDictionary());

    public static Error QuantityMustBeGreaterThanQuantitySold(Quantity quantitySold) => Error.Failure(
        "The quantity must be greater than the quantity sold",
        code: "ticket.quantity_must_be_greater_than_quantity_sold",
        details: new Dictionary<string, object?> { ["quantity_sold"] = quantitySold.Value }.ToFrozenDictionary());
}