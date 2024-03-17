using Eventify.Application.Common.Validation;

namespace Eventify.Application.Events.Commands.UploadPoster;

internal sealed class UploadPosterCommandValidator : AbstractValidator<UploadPosterCommand>
{
    public UploadPosterCommandValidator()
    {
        RuleFor(x => x.EventId)
            .Guid();

        RuleFor(x => x.Poster)
            .Image();
    }
}