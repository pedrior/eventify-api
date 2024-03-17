using Eventify.Application.Common.Validation;

namespace Eventify.Application.Events.Commands.RemovePoster;

internal sealed class RemoveEventPosterCommandValidator : AbstractValidator<RemoveEventPosterCommand>
{
    public RemoveEventPosterCommandValidator()
    {
        RuleFor(x => x.EventId)
            .Guid();
    }
}