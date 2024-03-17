using Eventify.Application.Common.Mappings;
using Eventify.Application.Events.Common.Errors;
using Eventify.Application.Events.Common.Mappings;
using Eventify.Application.Events.Queries.GetEvent;
using Eventify.Contracts.Events.Responses;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Producers;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Repository;

namespace Eventify.Application.UnitTests.Events.Queries.GetEvent;

[TestSubject(typeof(GetEventQueryHandler))]
public sealed class GetEventQueryHandlerTest
{
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
    private readonly IProducerRepository producerRepository = A.Fake<IProducerRepository>();
    private readonly ITicketRepository ticketRepository = A.Fake<ITicketRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly Event @event = Factories.Event.CreateEvent();
    private readonly Producer producer = Factories.Producer.CreateProducer();
    private readonly Ticket ticket = Factories.Ticket.CreateTicketValue();

    private readonly GetEventQuery query = new()
    {
        IdOrSlug = Constants.Event.Slug.Value
    };

    private readonly GetEventQueryHandler sut;

    public GetEventQueryHandlerTest()
    {
        TypeAdapterConfig.GlobalSettings.Apply(new CommonMappings());
        TypeAdapterConfig.GlobalSettings.Apply(new EventMappings());

        sut = new GetEventQueryHandler(eventRepository, producerRepository, ticketRepository);

        A.CallTo(() => eventRepository.GetPublishedAsync(new Slug(query.IdOrSlug), cancellationToken))
            .Returns(@event);

        A.CallTo(() => producerRepository.GetAsync(@event.ProducerId, cancellationToken))
            .Returns(producer);

        A.CallTo(() => ticketRepository.ListByEventAsync(@event.Id, cancellationToken))
            .Returns(new List<Ticket> { ticket });
    }

    [Fact]
    public async Task Handle_WhenEventDoesNotExistById_ShouldReturnNotFound()
    {
        // Arrange
        var idQuery = new GetEventQuery
        {
            IdOrSlug = Guid.NewGuid().ToString()
        };

        A.CallTo(() => eventRepository.GetPublishedAsync(@event.Id, cancellationToken))
            .Returns(null as Event);

        // Act
        var result = await sut.Handle(idQuery, cancellationToken);

        // Assert
        result.Should().BeError(EventErrors.NotFound(new EventId(Guid.Parse(idQuery.IdOrSlug))));
    }

    [Fact]
    public async Task Handle_WhenEventDoesNotExistBySlug_ShouldReturnNotFound()
    {
        // Arrange
        A.CallTo(() => eventRepository.GetPublishedAsync(new Slug(query.IdOrSlug), cancellationToken))
            .Returns(null as Event);

        // Act
        var result = await sut.Handle(query, cancellationToken);

        // Assert
        result.Should().BeError(EventErrors.NotFound(new Slug(query.IdOrSlug)));
    }

    [Fact]
    public async Task Handle_WhenProducerDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => producerRepository.GetAsync(@event.ProducerId, cancellationToken))
            .Returns(null as Producer);

        // Act
        var act = () => sut.Handle(query, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage($"Producer {@event.ProducerId} not found");
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldReturnEventResponse()
    {
        // Act
        var result = await sut.Handle(query, cancellationToken);

        // Assert
        result.Should().BeValue()
            .Which.Value.Should().BeEquivalentTo(@event.Adapt<EventResponse>() with
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
                Tickets =
                [
                    new EventTicketResponse
                    {
                        Id = ticket.Id.Value,
                        Name = ticket.Name,
                        Description = ticket.Description,
                        IsSoldOut = ticket.IsSoldOut,
                        Price = ticket.Price.Value,
                        Quantity = ticket.Quantity,
                        QuantitySold = ticket.QuantitySold,
                        QuantityPerSale = ticket.QuantityPerSale,
                        SaleStart = ticket.SaleStart,
                        SaleEnd = ticket.SaleEnd
                    }
                ]
            });
    }
}