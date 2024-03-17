using Eventify.Application.Common.Validation;

namespace Eventify.Application.Events.Commands.PublishEvent;

internal sealed class PublishEventCommandValidator : AbstractValidator<PublishEventCommand>
{
    public PublishEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .Guid();
    }
}