using Eventify.Application.Common.Abstractions.Files;
using Eventify.Application.Common.Abstractions.Persistence;
using Eventify.Application.Common.Abstractions.Requests;

namespace Eventify.Application.Producers.Commands.UploadPicture;

public sealed record UploadProducerPictureCommand : ICommand<Success>, ITransactional
{
    public required IFile Picture { get; init; }
}