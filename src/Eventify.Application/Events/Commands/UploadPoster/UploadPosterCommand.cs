using Eventify.Application.Common.Abstractions.Files;
using Eventify.Application.Common.Abstractions.Requests;

namespace Eventify.Application.Events.Commands.UploadPoster;

public sealed record UploadPosterCommand : ICommand<Success>
{
    public required Guid EventId { get; init; }
    
    public required IFile Poster { get; init; }
}