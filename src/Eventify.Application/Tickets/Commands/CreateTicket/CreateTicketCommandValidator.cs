using Eventify.Application.Common.Validation;
using Eventify.Application.Tickets.Common.Validations;

namespace Eventify.Application.Tickets.Commands.CreateTicket;

internal sealed class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(x => x.EventId)
            .Guid();

        RuleFor(x => x.Name)
            .TicketName();

        RuleFor(x => x.Description)
            .TicketDescription();

        RuleFor(x => x.Price)
            .Price();

        RuleFor(x => x.Quantity)
            .Quantity();

        RuleFor(x => x.QuantityPerSale)
            .Quantity()
            .GreaterThanOrEqualTo(1)
            .WithMessage("Must be greater than or equal to 1.");

        RuleFor(x => x.SaleStart)
            .TicketSaleDate();

        RuleFor(x => x.SaleEnd)
            .TicketSaleDate();
    }
}