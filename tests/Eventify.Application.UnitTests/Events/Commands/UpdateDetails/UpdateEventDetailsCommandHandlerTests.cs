using Eventify.Application.Events.Commands.UpdateDetails;
using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Common.Enums;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Enums;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Events.ValueObjects;

namespace Eventify.Application.UnitTests.Events.Commands.UpdateDetails;

[TestSubject(typeof(UpdateEventDetailsCommandHandler))]
public sealed class UpdateEventDetailsCommandHandlerTests
{
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly UpdateEventDetailsCommand command = new()
    {
        EventId = Guid.NewGuid(),
        Name = "Por que não gosto de Java?",
        Category = "conference",
        Language = "pt",
        Description = null
    };

    private readonly UpdateEventDetailsCommandHandler sut;

    public UpdateEventDetailsCommandHandlerTests()
    {
        sut = new UpdateEventDetailsCommandHandler(eventRepository);
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
    public async Task Handle_WhenEventExists_ShouldUpdateDetails()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        @event.Details.Should().Be(new EventDetails(
            name: command.Name,
            description: command.Description,
            category: EventCategory.FromName(command.Category),
            language: Language.FromName(command.Language)));
    }


    [Fact]
    public async Task Handle_WhenDetailsAreUpdate_ShouldUpdateEvent()
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
    public async Task Handle_WhenDetailsUpdatedSucceeds_ShouldReturnUpdated()
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
}