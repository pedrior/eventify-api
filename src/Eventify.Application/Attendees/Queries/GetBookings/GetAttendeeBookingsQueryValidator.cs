namespace Eventify.Application.Attendees.Queries.GetBookings;

internal sealed class GetAttendeeBookingsQueryValidator : AbstractValidator<GetAttendeeBookingsQuery>
{
    public GetAttendeeBookingsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Must be greater than or equal to 1");
        
        RuleFor(x => x.Limit)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Must be greater than or equal to 1");
    }
}