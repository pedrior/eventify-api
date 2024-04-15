using Eventify.Application.Common.Abstractions.Files;
using Eventify.Application.Common.Abstractions.Persistence;

namespace Eventify.Application.Attendees.Commands.UploadPicture;

public sealed record UploadAttendeePictureCommand : ICommand<Success>, ITransactional
{
    public required IFile Picture { get; init; }
}