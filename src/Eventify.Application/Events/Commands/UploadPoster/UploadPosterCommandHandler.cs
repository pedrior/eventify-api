using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Application.Common.Abstractions.Storage;
using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Events.Repository;

namespace Eventify.Application.Events.Commands.UploadPoster;

internal sealed class UploadPosterCommandHandler(
    IEventRepository eventRepository,
    IStorageService storageService
) : ICommandHandler<UploadPosterCommand, Success>
{
    public async Task<Result<Success>> Handle(UploadPosterCommand command,
        CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetAsync(command.EventId, cancellationToken);
        if (@event is null)
        {
            return EventErrors.NotFound(command.EventId);
        }

        var posterUrl = await storageService.UploadAsync(
            StorageKeys.EventPoster(@event.Id),
            command.Poster,
            cancellationToken);

        if (posterUrl is null)
        {
            return EventErrors.PosterUploadFailed;
        }

        @event.SetPoster(posterUrl);

        await eventRepository.UpdateAsync(@event, cancellationToken);

        return Success.Value;
    }
}