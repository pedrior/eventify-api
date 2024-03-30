using Eventify.Application.Events.Commands.UpdateLocation;
using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Events.ValueObjects;

namespace Eventify.Application.UnitTests.Events.Commands.UpdateLocation;

[TestSubject(typeof(UpdateEventLocationCommandHandler))]
public sealed class UpdateEventLocationCommandHandlerTests
{
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly UpdateEventLocationCommand command = new()
    {
        EventId = Guid.NewGuid(),
        Name = "Classic Hall",
        Address = "Av. Gov. Agamenon Magalhães, S/N - Salgadinho",
        ZipCode = "53110710",
        City = "Olinda",
        State = "PE",
        Country = "BR"
    };

    private readonly UpdateEventLocationCommandHandler sut;

    public UpdateEventLocationCommandHandlerTests()
    {
        sut = new UpdateEventLocationCommandHandler(eventRepository);
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
    public async Task Handle_WhenEventExists_ShouldUpdateLocation()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        @event.Location.Should().Be(new EventLocation(
            command.Name,
            command.Address,
            command.ZipCode,
            command.City,
            command.State,
            command.Country));
    }

    [Fact]
    public async Task Handle_WhenLocationAreUpdate_ShouldUpdateEvent()
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
    public async Task Handle_WhenLocationUpdatedSucceeds_ShouldReturnSuccess()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeSuccess(Success.Value);
    }

    [Fact]
    public async Task Handle_WhenLocationUpdateFails_ShouldReturnError()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent(tickets: 1);
        @event.Publish(); // Fail

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeFailure();
    }

    [Fact]
    public async Task Handle_WhenLocationUpdateFails_ShouldNotUpdateEvent()
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