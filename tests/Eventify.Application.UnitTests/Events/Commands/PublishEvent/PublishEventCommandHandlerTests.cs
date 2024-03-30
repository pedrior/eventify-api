using Eventify.Application.Events.Commands.PublishEvent;
using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Producers;
using Eventify.Domain.Producers.Repository;

namespace Eventify.Application.UnitTests.Events.Commands.PublishEvent;

[TestSubject(typeof(PublishEventCommandHandler))]
public sealed class PublishEventCommandHandlerTests
{
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
    private readonly IProducerRepository producerRepository = A.Fake<IProducerRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly Event @event = Factories.Event.CreateEvent();
    private readonly Producer producer = Factories.Producer.CreateProducer();

    private readonly PublishEventCommand command = new()
    {
        EventId = Constants.Event.EventId.Value
    };

    private readonly PublishEventCommandHandler sut;

    public PublishEventCommandHandlerTests()
    {
        sut = new PublishEventCommandHandler(eventRepository, producerRepository);

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
        result.Should().BeFailure(EventErrors.NotFound(command.EventId));
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
    public async Task Handle_WhenPublishSucceeds_ShouldReturnSuccess()
    {
        // Arrange
        @event.AddTicket(Factories.Ticket.CreateTicketValue());
        producer.AddEvent(@event);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeSuccess(Success.Value);
    }

    [Fact]
    public async Task Handle_WhenPublishSucceeds_ShouldUpdateEvent()
    {
        // Arrange
        @event.AddTicket(Factories.Ticket.CreateTicketValue());
        producer.AddEvent(@event);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => eventRepository.UpdateAsync(@event, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenPublishFails_ShouldReturnError()
    {
        // Arrange
        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeFailure();
    }
}