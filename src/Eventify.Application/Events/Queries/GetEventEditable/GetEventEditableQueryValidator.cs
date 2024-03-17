using Eventify.Application.Common.Validation;

namespace Eventify.Application.Events.Queries.GetEventEditable;

internal sealed class GetEventEditableQueryValidator : AbstractValidator<GetEventEditableQuery>
{
    public GetEventEditableQueryValidator()
    {
        RuleFor(x => x.EventId)
            .Guid();
    }
}