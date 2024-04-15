using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Contracts.Common.Responses;
using Eventify.Contracts.Events.Responses;

namespace Eventify.Application.Events.Queries.GetEvents;

public sealed record GetEventsQuery : IQuery<PageResponse<EventSummaryResponse>>
{
    public int Page { get; init; }

    public int Limit { get; init; }

    public string? Term { get; init; }
}