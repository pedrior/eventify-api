using Eventify.Application.Common.Abstractions.Storage;

namespace Eventify.Application.Events.Commands.UploadPoster;

public sealed record UploadPosterCommand : ICommand<Success>
{
    public required Guid EventId { get; init; }
    
    public required IFile Poster { get; init; }
}