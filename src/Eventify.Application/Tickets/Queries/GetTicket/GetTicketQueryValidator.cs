using Eventify.Application.Common.Validation;

namespace Eventify.Application.Tickets.Queries.GetTicket;

internal sealed class GetTicketQueryValidator : AbstractValidator<GetTicketQuery>
{
    public GetTicketQueryValidator()
    {
        RuleFor(x => x.TicketId)
            .Guid();
    }
}