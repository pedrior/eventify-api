using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Domain.Tickets.Errors;

internal static class TicketErrors
{
    public static readonly Error SoldOut = Error.Conflict(
        code: "ticket.sold_out",
        description: "The ticket is sold out");

    public static readonly Error NoSaleToCancel = Error.Conflict(
        code: "ticket.no_sale_to_cancel",
        description: "There is no sale to cancel");

    public static readonly Error QuantityPerSaleMustBeAtLeastOne = Error.Failure(
        code: "ticket.quantity_per_sale_must_be_at_least_one",
        description: "The quantity per sale must be equal to or greater than one");

    public static Error NotInSalePeriod(DateTimeOffset? saleStart, DateTimeOffset? saleEnd) => Error.Conflict(
        code: "ticket.not_in_sale_period",
        description: "The ticket is not in sale period",
        metadata: new()
        {
            ["sale_start"] = saleStart?.ToString("O") ?? "null",
            ["sale_end"] = saleEnd?.ToString("O") ?? "null"
        });

    public static Error QuantityMustBeGreaterThanQuantitySold(Quantity quantitySold) => Error.Failure(
        code: "ticket.quantity_must_be_greater_than_quantity_sold",
        description: "The quantity must be greater than the quantity sold",
        metadata: new() { ["quantity_sold"] = quantitySold.Value });
}