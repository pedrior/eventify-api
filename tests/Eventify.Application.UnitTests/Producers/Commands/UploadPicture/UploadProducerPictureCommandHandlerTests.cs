using Eventify.Application.Common.Abstractions.Files;
using Eventify.Application.Common.Abstractions.Storage;
using Eventify.Application.Producers.Commands.UploadPicture;
using Eventify.Application.Producers.Common.Errors;
using Eventify.Domain.Producers;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.UnitTests.Producers.Commands.UploadPicture;

[TestSubject(typeof(UploadProducerPictureCommandHandler))]
public sealed class UploadProducerPictureCommandHandlerTests
{
    private readonly IUser user = A.Fake<IUser>();
    private readonly IProducerRepository producerRepository = A.Fake<IProducerRepository>();
    private readonly IStorageService storageService = A.Fake<IStorageService>();
    
    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly UploadProducerPictureCommand command = new()
    {
        Picture = A.Dummy<IFile>()
    };

    private readonly UploadProducerPictureCommandHandler sut;

    public UploadProducerPictureCommandHandlerTests()
    {
        sut = new UploadProducerPictureCommandHandler(user, producerRepository, storageService);

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
    public async Task Handle_WhenProducerExists_ShouldUploadPictureToStorage()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer();

        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(producer);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => storageService.UploadAsync(
                StorageKeys.ProducerPicture(producer.Id),
                command.Picture,
                cancellationToken))
            .MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task Handle_WhenPictureUploadFails_ShouldReturnPictureUploadFailed()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer();
        var pictureKey = StorageKeys.ProducerPicture(producer.Id);

        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(producer);

        A.CallTo(() => storageService.UploadAsync(
                pictureKey,
                command.Picture,
                cancellationToken))
            .Returns(null as Uri);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeFailure(ProducerErrors.PictureUploadFailed);
    }

    [Fact]
    public async Task Handle_WhenPictureIsUploaded_ShouldUpdateProducerPictureUrl()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer();
        var pictureKey = StorageKeys.ProducerPicture(producer.Id);
        var pictureUrl = new Uri(new Uri("https://www.storage.com/"), pictureKey);

        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(producer);

        A.CallTo(() => storageService.UploadAsync(
                pictureKey,
                command.Picture,
                cancellationToken))
            .Returns(pictureUrl);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        producer.PictureUrl.Should().Be(pictureUrl);
    }

    [Fact]
    public async Task Handle_WhenPictureUrlIsUpdated_ShouldUpdateProducer()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer();
        var pictureKey = StorageKeys.ProducerPicture(producer.Id);
        var pictureUrl = new Uri(new Uri("https://www.storage.com/"), pictureKey);

        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(producer);

        A.CallTo(() => storageService.UploadAsync(
                pictureKey,
                command.Picture,
                cancellationToken))
            .Returns(pictureUrl);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => producerRepository.UpdateAsync(producer, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenProducerIsUpdated_ShouldReturnSuccess()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer();
        var pictureKey = StorageKeys.ProducerPicture(producer.Id);
        var pictureUrl = new Uri(new Uri("https://www.storage.com/"), pictureKey);

        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(producer);

        A.CallTo(() => storageService.UploadAsync(
                pictureKey,
                command.Picture,
                cancellationToken))
            .Returns(pictureUrl);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeSuccess(Success.Value);
    }
}