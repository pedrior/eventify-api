using Eventify.Application.Common.Validation;
using Eventify.Application.Events.Common.Validations;

namespace Eventify.Application.Events.Commands.UpdateLocation;

internal sealed class UpdateEventLocationCommandValidator : AbstractValidator<UpdateEventLocationCommand>
{
    public UpdateEventLocationCommandValidator()
    {
        RuleFor(x => x.EventId)
            .Guid();

        RuleFor(x => x.Name)
            .EventLocationName();

        RuleFor(x => x.Address)
            .EventLocationAddress();

        RuleFor(x => x.ZipCode)
            .EventLocationZipCode();

        RuleFor(x => x.City)
            .EventLocationCity();

        RuleFor(x => x.State)
            .EventLocationState();

        RuleFor(x => x.Country)
            .EventLocationCountry();
    }
}