using Eventify.Application.Events.Commands.UpdateSlug;
using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Events.Services;

namespace Eventify.Application.UnitTests.Events.Commands.UpdateSlug;

[TestSubject(typeof(UpdateEventSlugCommandHandler))]
public sealed class UpdateEventSlugCommandHandlerTests
{
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();

    private readonly IEventSlugUniquenessChecker eventSlugUniquenessChecker =
        A.Fake<IEventSlugUniquenessChecker>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly UpdateEventSlugCommand command = new()
    {
        EventId = Guid.NewGuid(),
        Slug = "my-awesome-new-slug"
    };

    private readonly UpdateEventSlugCommandHandler sut;

    public UpdateEventSlugCommandHandlerTests()
    {
        sut = new UpdateEventSlugCommandHandler(eventRepository, eventSlugUniquenessChecker);
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
    public async Task Handle_WhenUpdateSlugFails_ShouldReturnError()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        A.CallTo(() => eventSlugUniquenessChecker.IsUniqueAsync(new Slug(command.Slug), cancellationToken))
            .Returns(false);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeFailure();
    }

    [Fact]
    public async Task Handle_WhenUpdateSlugFails_ShouldNotUpdateEvent()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        A.CallTo(() => eventSlugUniquenessChecker.IsUniqueAsync(new Slug(command.Slug), cancellationToken))
            .Returns(false);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => eventRepository.UpdateAsync(@event, cancellationToken))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task Handle_WhenUpdateSlugSucceeds_ShouldUpdateSlug()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        A.CallTo(() => eventSlugUniquenessChecker.IsUniqueAsync(new Slug(command.Slug), cancellationToken))
            .Returns(true);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        @event.Slug.Should().Be(new Slug(command.Slug));
    }

    [Fact]
    public async Task Handle_WhenUpdateSlugSucceeds_ShouldReturnSuccess()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        A.CallTo(() => eventSlugUniquenessChecker.IsUniqueAsync(new Slug(command.Slug), cancellationToken))
            .Returns(true);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeSuccess(Success.Value);
    }

    [Fact]
    public async Task Handle_WhenUpdateSlugSucceeds_ShouldUpdateEvent()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        A.CallTo(() => eventSlugUniquenessChecker.IsUniqueAsync(new Slug(command.Slug), cancellationToken))
            .Returns(true);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => eventRepository.UpdateAsync(@event, cancellationToken))
            .MustHaveHappened();
    }
}