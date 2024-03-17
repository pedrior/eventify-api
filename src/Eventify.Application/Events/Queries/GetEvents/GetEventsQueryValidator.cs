namespace Eventify.Application.Events.Queries.GetEvents;

internal sealed class GetEventsQueryValidator : AbstractValidator<GetEventsQuery>
{
    public GetEventsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Must be greater than or equal to 1");
        
        RuleFor(x => x.Limit)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Must be greater than or equal to 1");
    }
}