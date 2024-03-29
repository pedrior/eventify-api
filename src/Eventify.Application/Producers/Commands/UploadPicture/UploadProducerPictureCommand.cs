using Eventify.Application.Common.Abstractions.Data;
using Eventify.Application.Common.Abstractions.Files;

namespace Eventify.Application.Producers.Commands.UploadPicture;

public sealed record UploadProducerPictureCommand : ICommand<Success>, ITransactional
{
    public required IFile Picture { get; init; }
}