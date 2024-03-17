using Eventify.Application.Events.Commands.UnpublishEvent;
using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Producers;
using Eventify.Domain.Producers.Repository;

namespace Eventify.Application.UnitTests.Events.Commands.UnpublishEvent;

[TestSubject(typeof(UnpublishEventCommandHandler))]
public sealed class UnpublishEventCommandHandlerTests
{
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
    private readonly IProducerRepository producerRepository = A.Fake<IProducerRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly Event @event = Factories.Event.CreateEvent();
    private readonly Producer producer = Factories.Producer.CreateProducer();

    private readonly UnpublishEventCommand command = new()
    {
        EventId = Constants.Event.EventId.Value
    };

    private readonly UnpublishEventCommandHandler sut;

    public UnpublishEventCommandHandlerTests()
    {
        sut = new UnpublishEventCommandHandler(eventRepository, producerRepository);

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        A.CallTo(() => producerRepository.GetAsync(@event.ProducerId, cancellationToken))
            .Returns(producer);
    }

    [Fact]
    public async Task Handle_WhenEventDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(null as Event);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeError(EventErrors.NotFound(command.EventId));
    }

    [Fact]
    public async Task Handle_WhenEventProducerDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => producerRepository.GetAsync(@event.ProducerId, cancellationToken))
            .Returns(null as Producer);

        // Act
        var act = () => sut.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ApplicationException>()
            .WithMessage($"Producer {@event.ProducerId} not found");
    }

    [Fact]
    public async Task Handle_WhenUnpublishSucceeds_ShouldReturnSuccess()
    {
        // Arrange
        @event.AddTicket(Factories.Ticket.CreateTicketValue());
        producer.AddEvent(@event);

        producer.PublishEvent(@event);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeValue(Result.Success);
    }

    [Fact]
    public async Task Handle_WhenUnpublishSucceeds_ShouldUpdateEvent()
    {
        // Arrange
        @event.AddTicket(Factories.Ticket.CreateTicketValue());
        producer.AddEvent(@event);

        producer.PublishEvent(@event);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => eventRepository.UpdateAsync(@event, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task Handle_WhenUnpublishFails_ShouldReturnError()
    {
        // Arrange
        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeError();
    }
}