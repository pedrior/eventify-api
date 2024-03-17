using Eventify.Application.Common.Validation;

namespace Eventify.Application.Events.Commands.UpdatePeriod;

internal sealed class UpdateEventPeriodCommandValidator : AbstractValidator<UpdateEventPeriodCommand>
{
    public UpdateEventPeriodCommandValidator()
    {
        RuleFor(x => x.EventId)
            .Guid();

        RuleFor(x => x.Start)
            .UtcDateTine();

        RuleFor(x => x.End)
            .UtcDateTine()
            .GreaterThan(x => x.Start)
            .WithMessage("Must be greater than start.");
    }
}