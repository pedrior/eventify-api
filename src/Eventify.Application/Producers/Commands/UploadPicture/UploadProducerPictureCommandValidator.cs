using Eventify.Application.Common.Validation;

namespace Eventify.Application.Producers.Commands.UploadPicture;

internal sealed class UploadProducerPictureCommandValidator : AbstractValidator<UploadProducerPictureCommand>
{
    public UploadProducerPictureCommandValidator()
    {
        RuleFor(x => x.Picture)
            .Image();
    }
}