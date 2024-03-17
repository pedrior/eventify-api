using Eventify.Application.Events.Commands.UpdatePeriod;
using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;

namespace Eventify.Application.UnitTests.Events.Commands.UpdatePeriod;

[TestSubject(typeof(UpdateEventPeriodCommandHandler))]
public sealed class UpdateEventPeriodCommandHandlerTests
{
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly UpdateEventPeriodCommand command = new()
    {
        EventId = Guid.NewGuid(),
        Start = DateTimeOffset.UtcNow,
        End = DateTimeOffset.UtcNow.AddHours(2)
    };

    private readonly UpdateEventPeriodCommandHandler sut;

    public UpdateEventPeriodCommandHandlerTests()
    {
        sut = new UpdateEventPeriodCommandHandler(eventRepository);
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
    public async Task Handle_WhenEventExists_ShouldUpdatePeriod()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        @event.Period.Should().Be(new Period(command.Start, command.End));
    }

    [Fact]
    public async Task Handle_WhenPeriodAreUpdate_ShouldUpdateEvent()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => eventRepository.UpdateAsync(@event, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenPeriodUpdatedSucceeds_ShouldReturnUpdated()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeValue(Result.Updated);
    }

    [Fact]
    public async Task Handle_WhenPeriodUpdateFails_ShouldReturnError()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent(tickets: 1);
        @event.Publish(); // Fail

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeError();
    }

    [Fact]
    public async Task Handle_WhenPeriodUpdateFails_ShouldNotUpdateEvent()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent(tickets: 1);
        @event.Publish(); // Fail

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => eventRepository.UpdateAsync(@event, cancellationToken))
            .MustNotHaveHappened();
    }
}