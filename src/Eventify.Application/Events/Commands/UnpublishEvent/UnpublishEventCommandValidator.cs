using Eventify.Application.Common.Validation;

namespace Eventify.Application.Events.Commands.UnpublishEvent;

internal sealed class UnpublishEventCommandValidator : AbstractValidator<UnpublishEventCommand>
{
    public UnpublishEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .Guid();
    }
}