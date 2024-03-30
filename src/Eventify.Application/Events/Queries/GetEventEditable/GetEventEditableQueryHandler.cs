using Eventify.Application.Events.Common.Errors;
using Eventify.Contracts.Events.Responses;
using Eventify.Domain.Events.Repository;

namespace Eventify.Application.Events.Queries.GetEventEditable;

internal sealed class GetEventEditableQueryHandler(IEventRepository eventRepository)
    : IQueryHandler<GetEventEditableQuery, EventEditResponse>
{
    public async Task<Result<EventEditResponse>> Handle(GetEventEditableQuery query,
        CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetAsync(query.EventId, cancellationToken);
        return @event is not null
            ? @event.Adapt<EventEditResponse>()
            : EventErrors.NotFound(query.EventId);
    }
}