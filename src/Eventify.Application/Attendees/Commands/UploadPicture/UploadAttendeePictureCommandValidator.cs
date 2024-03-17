using Eventify.Application.Common.Validation;

namespace Eventify.Application.Attendees.Commands.UploadPicture;

internal sealed class UploadAttendeePictureCommandValidator : AbstractValidator<UploadAttendeePictureCommand>
{
    public UploadAttendeePictureCommandValidator()
    {
        RuleFor(x => x.Picture)
            .Image();
    }
}