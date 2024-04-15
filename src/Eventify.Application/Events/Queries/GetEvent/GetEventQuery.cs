using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Contracts.Events.Responses;

namespace Eventify.Application.Events.Queries.GetEvent;

public sealed record GetEventQuery : IQuery<EventResponse>
{
    public required string IdOrSlug { get; init; }
}