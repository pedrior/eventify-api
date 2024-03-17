using Eventify.Application.Common.Validation;

namespace Eventify.Application.Tickets.Commands.RemoveTicket;

internal sealed class RemoveTicketCommandValidator : AbstractValidator<RemoveTicketCommand>
{
    public RemoveTicketCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .Guid();
    }
}