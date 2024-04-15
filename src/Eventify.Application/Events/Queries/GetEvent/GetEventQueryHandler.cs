using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Application.Events.Common.Errors;
using Eventify.Contracts.Events.Responses;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Tickets.Repository;

namespace Eventify.Application.Events.Queries.GetEvent;

internal sealed class GetEventQueryHandler(
    IEventRepository eventRepository,
    IProducerRepository producerRepository,
    ITicketRepository ticketRepository
) : IQueryHandler<GetEventQuery, EventResponse>
{
    public async Task<Result<EventResponse>> Handle(GetEventQuery query,
        CancellationToken cancellationToken)
    {
        var eventResult = await GetEventAsync(query.IdOrSlug, cancellationToken);
        if (eventResult.IsFailure)
        {
            return eventResult.Errors;
        }

        var @event = eventResult.Value;

        var producer = await producerRepository.GetAsync(@event.ProducerId, cancellationToken);
        if (producer is null)
        {
            throw new ApplicationException($"Producer {@event.ProducerId} not found");
        }

        var tickets = await ticketRepository.ListByEventAsync(@event.Id, cancellationToken);

        return @event.Adapt<EventResponse>() with
        {
            Producer = new EventProducerResponse
            {
                Name = producer.Details.Name,
                Bio = producer.Details.Bio,
                Email = producer.Contact.Email,
                PhoneNumber = producer.Contact.PhoneNumber,
                PictureUrl = producer.PictureUrl?.ToString(),
                WebsiteUrl = producer.WebsiteUrl?.ToString()
            },
            Tickets = tickets.Select(t => new EventTicketResponse
            {
                Id = t.Id.Value,
                Name = t.Name,
                Description = t.Description,
                IsSoldOut = t.IsSoldOut,
                Price = t.Price.Value,
                Quantity = t.Quantity,
                QuantitySold = t.QuantitySold,
                QuantityPerSale = t.QuantityPerSale,
                SaleStart = t.SaleStart,
                SaleEnd = t.SaleEnd
            })
        };
    }

    private async Task<Result<Event>> GetEventAsync(string idOrSlug, CancellationToken cancellationToken)
    {
        if (Guid.TryParse(idOrSlug, out var id))
        {
            var eventById = await eventRepository.GetPublishedAsync(id, cancellationToken);
            return eventById is null ? EventErrors.NotFound(id) : eventById;
        }

        var slug = new Slug(idOrSlug);
        var eventBySlug = await eventRepository.GetPublishedAsync(slug, cancellationToken);
        return eventBySlug is null ? EventErrors.NotFound(slug) : eventBySlug;
    }
}