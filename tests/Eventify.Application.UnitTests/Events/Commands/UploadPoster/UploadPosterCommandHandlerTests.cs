using Eventify.Application.Common.Abstractions.Storage;
using Eventify.Application.Events.Commands.UploadPoster;
using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;

namespace Eventify.Application.UnitTests.Events.Commands.UploadPoster;

[TestSubject(typeof(UploadPosterCommandHandler))]
public sealed class UploadPosterCommandHandlerTests
{
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
    private readonly IStorageService storageService = A.Fake<IStorageService>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly UploadPosterCommand command = new()
    {
        EventId = Guid.NewGuid(),
        Poster = A.Dummy<IFile>()
    };

    private readonly UploadPosterCommandHandler sut;

    public UploadPosterCommandHandlerTests()
    {
        sut = new UploadPosterCommandHandler(eventRepository, storageService);
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
    public async Task Handle_WhenEventExists_ShouldUploadPosterToStorage()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();
        var posterKey = StorageKeys.EventPoster(@event.Id);

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => storageService.UploadAsync(
                posterKey,
                command.Poster,
                cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenPosterIsUploaded_ShouldSetEventPoster()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();
        var posterKey = StorageKeys.EventPoster(@event.Id);
        var posterUrl = new Uri(new Uri("https://www.storage.com/"), posterKey);

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        A.CallTo(() => storageService.UploadAsync(
                posterKey,
                command.Poster,
                cancellationToken))
            .Returns(posterUrl);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        @event.PosterUrl.Should().Be(posterUrl);
    }

    [Fact]
    public async Task Handle_WhenEventPosterIsSet_ShouldUpdateEvent()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();
        var posterKey = StorageKeys.EventPoster(@event.Id);
        var posterUrl = new Uri(new Uri("https://www.storage.com/"), posterKey);

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        A.CallTo(() => storageService.UploadAsync(
                posterKey,
                command.Poster,
                cancellationToken))
            .Returns(posterUrl);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => eventRepository.UpdateAsync(@event, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenEventPosterIsSet_ShouldReturnSuccess()
    {
        // Arrange
        var @event = Factories.Event.CreateEvent();
        var posterKey = StorageKeys.EventPoster(@event.Id);
        var posterUrl = new Uri(new Uri("https://www.storage.com/"), posterKey);

        A.CallTo(() => eventRepository.GetAsync(command.EventId, cancellationToken))
            .Returns(@event);

        A.CallTo(() => storageService.UploadAsync(
                posterKey,
                command.Poster,
                cancellationToken))
            .Returns(posterUrl);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeSuccess(Success.Value);
    }
}