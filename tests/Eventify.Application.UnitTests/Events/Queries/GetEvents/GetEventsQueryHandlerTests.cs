using Eventify.Application.Common.Mappings;
using Eventify.Application.Events.Common.Mappings;
using Eventify.Application.Events.Queries.GetEvents;
using Eventify.Contracts.Events.Responses;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Producers;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Producers.ValueObjects;

namespace Eventify.Application.UnitTests.Events.Queries.GetEvents;

[TestSubject(typeof(GetEventsQueryHandler))]
public sealed class GetEventsQueryHandlerTests
{
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
    private readonly IProducerRepository producerRepository = A.Fake<IProducerRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly GetEventsQuery query = new()
    {
        Page = 1,
        Limit = 10
    };

    private readonly GetEventsQueryHandler sut;

    public GetEventsQueryHandlerTests()
    {
        TypeAdapterConfig.GlobalSettings.Apply(new CommonMappings());
        TypeAdapterConfig.GlobalSettings.Apply(new EventMappings());

        sut = new GetEventsQueryHandler(eventRepository, producerRepository);
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldReturnPageResponse()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer();

        Event[] events =
        [
            Factories.Event.CreateEvent(),
            Factories.Event.CreateEvent(),
            Factories.Event.CreateEvent()
        ];

        A.CallTo(() => eventRepository.ListPublishedAsync(
            query.Page,
            query.Limit,
            query.Term,
            cancellationToken
        )).Returns(events);

        A.CallTo(() => eventRepository.CountPublishedAsync(query.Term, cancellationToken))
            .Returns(events.Length);

        A.CallTo(() => producerRepository.GetAsync(A<ProducerId>._, cancellationToken))
            .Returns(producer);

        // Act
        var result = await sut.Handle(query, cancellationToken);

        // Assert
        result.Should().BeValue()
            .Which.Value.Should().BeEquivalentTo(new
            {
                query.Page,
                query.Limit,
                Total = events.Length,
                Items = events.Select(e => e.Adapt<EventSummaryResponse>() with
                {
                    ProducerName = producer.Details.Name,
                    ProducerPictureUrl = producer.PictureUrl?.ToString()
                })
            });
    }

    [Fact]
    public async Task Handle_WhenEventProducerDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();

        A.CallTo(() => eventRepository.ListPublishedAsync(
            query.Page,
            query.Limit,
            query.Term,
            cancellationToken
        )).Returns(new[] { @event });

        A.CallTo(() => eventRepository.CountPublishedAsync(query.Term, cancellationToken))
            .Returns(1);

        A.CallTo(() => producerRepository.GetAsync(A<ProducerId>._, cancellationToken))
            .Returns(null as Producer);

        // Act
        var act = () => sut.Handle(query, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ApplicationException>()
            .WithMessage($"Producer {@event.ProducerId} not found");
    }
}