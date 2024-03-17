using Eventify.Application.Common.Abstractions.Storage;
using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Events.Repository;

namespace Eventify.Application.Events.Commands.RemovePoster;

internal sealed class RemoveEventPosterCommandHandler(
    IEventRepository eventRepository,
    IStorageService storageService
) : ICommandHandler<RemoveEventPosterCommand, Deleted>
{
    public async Task<ErrorOr<Deleted>> Handle(RemoveEventPosterCommand command,
        CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetAsync(command.EventId, cancellationToken);
        if (@event is null)
        {
            return EventErrors.NotFound(command.EventId);
        }

        if (@event.PosterUrl is null)
        {
            return Result.Deleted;
        }

        var posterKey = StorageKeys.EventPoster(@event.Id);
        var deleted = await storageService.DeleteAsync(posterKey, cancellationToken);
        if (!deleted)
        {
            return EventErrors.PosterDeletionFailed;
        }

        @event.RemovePoster();

        await eventRepository.UpdateAsync(@event, cancellationToken);

        return Result.Deleted;
    }
}