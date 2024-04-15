using Eventify.Application.Common.Abstractions.Files;
using Eventify.Application.Common.Abstractions.Persistence;
using Eventify.Application.Common.Abstractions.Requests;

namespace Eventify.Application.Attendees.Commands.UploadPicture;

public sealed record UploadAttendeePictureCommand : ICommand<Success>, ITransactional
{
    public required IFile Picture { get; init; }
}