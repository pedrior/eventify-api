using Eventify.Contracts.Events.Responses;

namespace Eventify.Application.Events.Queries.GetEventEditable;

public sealed record GetEventEditableQuery : IQuery<EventEditResponse>
{
    public required Guid EventId { get; init; }
}