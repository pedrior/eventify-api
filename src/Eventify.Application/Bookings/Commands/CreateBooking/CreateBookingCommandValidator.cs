using Eventify.Application.Common.Validation;

namespace Eventify.Application.Bookings.Commands.CreateBooking;

internal sealed class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .Guid();
    }
}