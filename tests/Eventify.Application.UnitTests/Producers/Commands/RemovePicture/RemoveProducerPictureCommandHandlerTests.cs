using Eventify.Application.Producers.Commands.RemovePicture;
using Eventify.Application.Producers.Common.Errors;
using Eventify.Application.Common.Abstractions.Storage;
using Eventify.Domain.Producers;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.UnitTests.Producers.Commands.RemovePicture;

[TestSubject(typeof(RemoveProducerPictureCommandHandler))]
public sealed class RemoveProducerPictureCommandHandlerTests
{
    private readonly IUser user = A.Fake<IUser>();
    private readonly IProducerRepository producerRepository = A.Fake<IProducerRepository>();
    private readonly IStorageService storageService = A.Fake<IStorageService>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly RemoveProducerPictureCommand command = new();

    private readonly RemoveProducerPictureCommandHandler sut;

    public RemoveProducerPictureCommandHandlerTests()
    {
        sut = new RemoveProducerPictureCommandHandler(user, producerRepository, storageService);

        A.CallTo(() => user.Id)
            .Returns(Constants.UserId);
    }

    [Fact]
    public async Task Handle_WhenProducerDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(null as Producer);

        // Act
        var act = () => sut.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage("Producer not found");
    }

    [Fact]
    public async Task Handle_WhenProducerDoesNotHavePicture_ShouldReturnDeleted()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer();

        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(producer);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeValue(Result.Deleted);

        A.CallTo(() => storageService.DeleteAsync(A<Uri>._, cancellationToken))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task Handle_WhenProducerHasPicture_ShouldDeletePicture()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer(
            pictureUrl: new Uri("https://www.some-producer-picture.com/picture"));

        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(producer);

        A.CallTo(() => storageService.DeleteAsync(StorageKeys.ProducerPicture(producer.Id), cancellationToken))
            .Returns(true);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        producer.PictureUrl.Should().BeNull();

        A.CallTo(() => storageService.DeleteAsync(StorageKeys.ProducerPicture(producer.Id), cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenProducerPictureDeletionFails_ShouldReturnPictureDeletionFailed()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer(
            pictureUrl: new Uri("https://www.some-producer-picture.com/picture"));

        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(producer);

        A.CallTo(() => storageService.DeleteAsync(StorageKeys.ProducerPicture(producer.Id), cancellationToken))
            .Returns(false);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeError(ProducerErrors.PictureDeletionFailed);
    }

    [Fact]
    public async Task Handle_WhenProducerPictureIsDeleted_ShouldUpdateProducer()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer(
            pictureUrl: new Uri("https://www.some-producer-picture.com/picture"));

        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(producer);

        A.CallTo(() => storageService.DeleteAsync(StorageKeys.ProducerPicture(producer.Id), cancellationToken))
            .Returns(true);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => producerRepository.UpdateAsync(producer, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }
}