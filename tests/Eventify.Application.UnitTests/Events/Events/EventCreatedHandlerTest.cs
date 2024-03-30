using Eventify.Application.Events.Events;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Events;
using Eventify.Domain.Producers;
using Eventify.Domain.Producers.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.UnitTests.Events.Events;

[TestSubject(typeof(EventCreatedHandler))]
public sealed class EventCreatedHandlerTest
{
    private readonly IProducerRepository producerRepository = A.Fake<IProducerRepository>();
    private readonly ILogger<EventCreatedHandler> logger = A.Fake<ILogger<EventCreatedHandler>>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly Event @event = Factories.Event.CreateEvent();
    private readonly Producer producer = Factories.Producer.CreateProducer();

    private readonly EventCreated e;

    private readonly EventCreatedHandler sut;

    public EventCreatedHandlerTest()
    {
        e = new EventCreated(@event);
        sut = new EventCreatedHandler(producerRepository, logger);

        A.CallTo(() => producerRepository.GetAsync(@event.ProducerId, cancellationToken))
            .Returns(producer);
    }

    [Fact]
    public async Task Handle_WhenEventProducerDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => producerRepository.GetAsync(@event.ProducerId, cancellationToken))
            .Returns(null as Producer);

        // Act
        var act = () => sut.Handle(e, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ApplicationException>()
            .WithMessage($"Producer {@event.ProducerId} not found");
    }

    [Fact]
    public async Task Handle_WhenAddingEventToProducerSucceeds_ShouldUpdateProducer()
    {
        // Act
        await sut.Handle(e, cancellationToken);

        // Assert
        A.CallTo(() => producerRepository.UpdateAsync(producer, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenAddingEventToProducerFails_ShouldThrowResultException()
    {
        // Arrange
        producer.AddEvent(@event);

        // Act
        var act = () => sut.Handle(e, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ResultException>();
    }
}