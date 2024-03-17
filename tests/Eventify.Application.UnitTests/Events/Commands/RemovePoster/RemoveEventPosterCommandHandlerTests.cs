using Eventify.Application.Common.Abstractions.Storage;
using Eventify.Application.Events.Commands.RemovePoster;
using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;

namespace Eventify.Application.UnitTests.Events.Commands.RemovePoster;

[TestSubject(typeof(RemoveEventPosterCommandHandler))]
public sealed class RemoveEventPosterCommandHandlerTests
{
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
    private readonly IStorageService storageService = A.Fake<IStorageService>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly RemoveEventPosterCommand command = new()
    {
        EventId = Guid.NewGuid()
    };

    private readonly RemoveEventPosterCommandHandler sut;

    public RemoveEventPosterCommandHandlerTests()
    {
        sut = new RemoveEventPosterCommandHandler(eventRepository, storageService);
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
    public async Task Handle_WhenEventDoesNotHavePoster_ShouldReturnDeleted()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeValue(Result.Deleted);
    }

    [Fact]
    public async Task Handle_WhenEventDoesNotHavePoster_ShouldNotDeletePosterInStorage()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => storageService.DeleteAsync(A<Uri>._, cancellationToken))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task Handle_WhenEventHasPoster_ShouldDeletePosterInStorage()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent(
            posterUrl: new Uri("https://www.some-event.com/poster"));

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => storageService.DeleteAsync(StorageKeys.EventPoster(@event.Id), cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenEventHasPoster_ShouldUpdateEventPosterUrlToNull()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent(
            posterUrl: new Uri("https://www.some-event.com/poster"));

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);
        
        A.CallTo(() => storageService.DeleteAsync(StorageKeys.EventPoster(@event.Id), cancellationToken))
            .Returns(true);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        @event.PosterUrl.Should().BeNull();
    }

    [Fact]
    public async Task Handle_WhenEventHasPoster_ShouldUpdateEvent()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent(
            posterUrl: new Uri("https://www.some-event.com/poster"));

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);
        
        A.CallTo(() => storageService.DeleteAsync(StorageKeys.EventPoster(@event.Id), cancellationToken))
            .Returns(true);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => eventRepository.UpdateAsync(@event, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenEventHasPoster_ShouldReturnDeleted()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent(
            posterUrl: new Uri("https://www.some-event.com/poster"));

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);
        
        A.CallTo(() => storageService.DeleteAsync(StorageKeys.EventPoster(@event.Id), cancellationToken))
            .Returns(true);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeValue(Result.Deleted);
    }
    
    [Fact]
    public async Task Handle_WhenPosterDeletionFails_ShouldReturnPosterDeletionFailed()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent(
            posterUrl: new Uri("https://www.some-event.com/poster"));

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);
        
        A.CallTo(() => storageService.DeleteAsync(StorageKeys.EventPoster(@event.Id), cancellationToken))
            .Returns(false);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeError(EventErrors.PosterDeletionFailed);
    }
}