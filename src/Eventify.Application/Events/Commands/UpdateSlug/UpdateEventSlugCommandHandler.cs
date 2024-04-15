using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Events.Services;

namespace Eventify.Application.Events.Commands.UpdateSlug;

internal sealed class UpdateEventSlugCommandHandler(
    IEventRepository eventRepository,
    IEventSlugUniquenessChecker eventSlugUniquenessChecker
) : ICommandHandler<UpdateEventSlugCommand, Success>
{
    public async Task<Result<Success>> Handle(UpdateEventSlugCommand command,
        CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetAsync(command.EventId, cancellationToken);
        if (@event is null)
        {
            return EventErrors.NotFound(command.EventId);
        }

        var result = await @event.UpdateSlugAsync(
            new Slug(command.Slug),
            eventSlugUniquenessChecker,
            cancellationToken);
        
        return await result.ThenAsync(_ => eventRepository.UpdateAsync(@event, cancellationToken));
    }
}