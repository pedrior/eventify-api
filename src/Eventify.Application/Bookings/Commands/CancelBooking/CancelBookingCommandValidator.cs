using Eventify.Application.Common.Validation;

namespace Eventify.Application.Bookings.Commands.CancelBooking;

internal sealed class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .Guid();
    }
}