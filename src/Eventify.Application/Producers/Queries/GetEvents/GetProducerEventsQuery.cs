using Eventify.Contracts.Producers.Responses;

namespace Eventify.Application.Producers.Queries.GetEvents;

public sealed record GetProducerEventsQuery : IQuery<ProducerEventsResponse>;