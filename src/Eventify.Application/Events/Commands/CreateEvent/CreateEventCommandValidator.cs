using Eventify.Application.Common.Validation;
using Eventify.Application.Events.Common.Validations;

namespace Eventify.Application.Events.Commands.CreateEvent;

internal sealed class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    public CreateEventCommandValidator()
    {
        RuleFor(x => x.Name)
            .EventName();

        RuleFor(x => x.Category)
            .EventCategory();

        RuleFor(x => x.Language)
            .Language();
        
        RuleFor(x => x.Description)
            .EventDescription()
            .Unless(x => x.Description is null);

        RuleFor(x => x.PeriodStart)
            .UtcDateTine();

        RuleFor(x => x.PeriodEnd)
            .UtcDateTine()
            .GreaterThan(x => x.PeriodStart)
            .WithMessage("Must be greater than period start.");

        RuleFor(x => x.LocationName)
            .EventLocationName();

        RuleFor(x => x.LocationAddress)
            .EventLocationAddress();

        RuleFor(x => x.LocationZipCode)
            .EventLocationZipCode();

        RuleFor(x => x.LocationCity)
            .EventLocationCity();

        RuleFor(x => x.LocationState)
            .EventLocationState();

        RuleFor(x => x.LocationCountry)
            .EventLocationCountry();
    }
}