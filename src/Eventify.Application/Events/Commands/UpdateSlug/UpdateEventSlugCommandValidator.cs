using Eventify.Application.Common.Validation;
using Eventify.Application.Events.Common.Validations;

namespace Eventify.Application.Events.Commands.UpdateSlug;

internal sealed class UpdateEventSlugCommandValidator : AbstractValidator<UpdateEventSlugCommand>
{
    public UpdateEventSlugCommandValidator()
    {
        RuleFor(x => x.EventId)
            .Guid();

        RuleFor(x => x.Slug)
            .EventSlug();
    }
}