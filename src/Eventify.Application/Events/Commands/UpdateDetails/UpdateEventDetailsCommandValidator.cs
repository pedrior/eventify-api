using Eventify.Application.Common.Validation;
using Eventify.Application.Events.Common.Validations;

namespace Eventify.Application.Events.Commands.UpdateDetails;

internal sealed class UpdateEventDetailsCommandValidator : AbstractValidator<UpdateEventDetailsCommand>
{
    public UpdateEventDetailsCommandValidator()
    {
        RuleFor(x => x.EventId)
            .Guid();

        RuleFor(x => x.Name)
            .EventName();

        RuleFor(x => x.Category)
            .EventCategory();

        RuleFor(x => x.Language)
            .Language();

        RuleFor(x => x.Description)
            .EventDescription()
            .Unless(x => x.Description is null);
    }
}