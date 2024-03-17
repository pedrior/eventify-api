using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Common.Enums;
using Eventify.Domain.Events.Enums;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Events.ValueObjects;

namespace Eventify.Application.Events.Commands.UpdateDetails;

internal sealed class UpdateEventDetailsCommandHandler(IEventRepository eventRepository)
    : ICommandHandler<UpdateEventDetailsCommand, Updated>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateEventDetailsCommand command,
        CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetAsync(command.EventId, cancellationToken);
        if (@event is null)
        {
            return EventErrors.NotFound(command.EventId);
        }

        var details = new EventDetails(
            name: command.Name,
            description: command.Description,
            category: EventCategory.FromName(command.Category),
            language: Language.FromName(command.Language));

        return await @event.UpdateDetails(details)
            .ThenAsync(_ => eventRepository.UpdateAsync(@event, cancellationToken));
    }
}