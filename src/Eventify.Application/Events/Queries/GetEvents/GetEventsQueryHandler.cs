using Eventify.Contracts.Common.Responses;
using Eventify.Contracts.Events.Responses;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Producers.Repository;

namespace Eventify.Application.Events.Queries.GetEvents;

internal sealed class GetEventsQueryHandler(
    IEventRepository eventRepository,
    IProducerRepository producerRepository
) : IQueryHandler<GetEventsQuery, PageResponse<EventSummaryResponse>>
{
    public async Task<Result<PageResponse<EventSummaryResponse>>> Handle(GetEventsQuery query,
        CancellationToken cancellationToken)
    {
        var total = await eventRepository.CountPublishedAsync(query.Term, cancellationToken);
        var events = await eventRepository.ListPublishedAsync(
            query.Page,
            query.Limit,
            query.Term,
            cancellationToken);

        var items = new List<EventSummaryResponse>();
        foreach (var @event in events)
        {
            var producer = await producerRepository.GetAsync(@event.ProducerId, cancellationToken);
            if (producer is null)
            {
                throw new ApplicationException($"Producer {@event.ProducerId} not found");
            }

            items.Add(@event.Adapt<EventSummaryResponse>() with
            {
                ProducerName = producer.Details.Name,
                ProducerPictureUrl = producer.PictureUrl?.ToString()
            });
        }

        return new PageResponse<EventSummaryResponse>
        {
            Page = query.Page,
            Limit = query.Limit,
            Total = total,
            Items = items
        };
    }
}