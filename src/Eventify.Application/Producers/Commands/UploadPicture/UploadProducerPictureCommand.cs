using Eventify.Application.Common.Abstractions.Storage;

namespace Eventify.Application.Producers.Commands.UploadPicture;

public sealed record UploadProducerPictureCommand : ICommand<Success>, ITransactional
{
    public required IFile Picture { get; init; }
}