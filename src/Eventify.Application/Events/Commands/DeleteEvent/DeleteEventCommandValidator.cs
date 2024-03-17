using Eventify.Application.Common.Validation;

namespace Eventify.Application.Events.Commands.DeleteEvent;

internal sealed class DeleteEventCommandValidator : AbstractValidator<DeleteEventCommand>
{
    public DeleteEventCommandValidator()
    {
        RuleFor(x => x.EventId)
            .Guid();
    }
}