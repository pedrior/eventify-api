using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Application.Events.Queries.GetEvent;

internal sealed class GetEventQueryValidator : AbstractValidator<GetEventQuery>
{
    public GetEventQueryValidator()
    {
        RuleFor(x => x.IdOrSlug)
            .NotEmpty()
            .WithMessage("Must not be empty.")
            .Must(v => Guid.TryParse(v, out _) || Slug.IsValid(v))
            .WithMessage("Must be a valid GUID or Slug.");
    }
}