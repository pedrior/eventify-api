using Eventify.Application.Common.Abstractions.Files;

namespace Eventify.Application.Attendees.Commands.UploadPicture;

public sealed record UploadAttendeePictureCommand : ICommand<Success>, ITransactional
{
    public required IFile Picture { get; init; }
}