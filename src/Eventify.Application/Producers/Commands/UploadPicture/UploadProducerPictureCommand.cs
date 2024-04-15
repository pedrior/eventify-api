using Eventify.Application.Common.Abstractions.Files;
using Eventify.Application.Common.Abstractions.Persistence;

namespace Eventify.Application.Producers.Commands.UploadPicture;

public sealed record UploadProducerPictureCommand : ICommand<Success>, ITransactional
{
    public required IFile Picture { get; init; }
}