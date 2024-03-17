using Eventify.Application.Common.Validation;

namespace Eventify.Application.Bookings.Queries.GetBooking;

internal sealed class GetBookingQueryValidator : AbstractValidator<GetBookingQuery>
{
    public GetBookingQueryValidator()
    {
        RuleFor(x => x.BookingId)
            .Guid();
    }
}