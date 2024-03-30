using Eventify.Application.Attendees.Commands.UploadPicture;
using Eventify.Application.Attendees.Common.Errors;
using Eventify.Application.Common.Abstractions.Files;
using Eventify.Application.Common.Abstractions.Storage;
using Eventify.Domain.Attendees;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.UnitTests.Attendees.Commands.UploadPicture;

[TestSubject(typeof(UploadAttendeePictureCommandHandler))]
public sealed class UploadAttendeePictureCommandHandlerTests
{
    private readonly IUser user = A.Fake<IUser>();
    private readonly IAttendeeRepository attendeeRepository = A.Fake<IAttendeeRepository>();
    private readonly IStorageService storageService = A.Fake<IStorageService>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly UploadAttendeePictureCommand command = new()
    {
        Picture = A.Dummy<IFile>()
    };

    private readonly UploadAttendeePictureCommandHandler sut;

    public UploadAttendeePictureCommandHandlerTests()
    {
        sut = new UploadAttendeePictureCommandHandler(user, attendeeRepository, storageService);

        A.CallTo(() => user.Id)
            .Returns(Constants.UserId);
    }

    [Fact]
    public async Task Handle_WhenAttendeeDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(null as Attendee);

        // Act
        var act = () => sut.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ApplicationException>()
            .WithMessage("Attendee not found");
    }

    [Fact]
    public async Task Handle_WhenAttendeeExists_ShouldUploadPictureToStorage()
    {
        // Arrange
        var attendee = Factories.Attendee.CreateAttendee();
        var pictureKey = StorageKeys.AttendeePicture(attendee.Id);

        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(attendee);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => storageService.UploadAsync(
                pictureKey,
                command.Picture,
                cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenPictureUploadFails_ShouldReturnPictureUploadFailed()
    {
        // Arrange
        var attendee = Factories.Attendee.CreateAttendee();
        var pictureKey = StorageKeys.AttendeePicture(attendee.Id);

        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(attendee);

        A.CallTo(() => storageService.UploadAsync(
                pictureKey,
                command.Picture,
                cancellationToken))
            .Returns(null as Uri);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeFailure(AttendeeErrors.PictureUploadFailed);
    }

    [Fact]
    public async Task Handle_WhenPictureIsUploaded_ShouldUpdateAttendeePictureUrl()
    {
        // Arrange
        var attendee = Factories.Attendee.CreateAttendee();
        var pictureKey = StorageKeys.AttendeePicture(attendee.Id);
        var pictureUrl = new Uri(new Uri("https://www.storage.com/"), pictureKey);

        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(attendee);

        A.CallTo(() => storageService.UploadAsync(
                pictureKey,
                command.Picture,
                cancellationToken))
            .Returns(pictureUrl);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        attendee.PictureUrl.Should().Be(pictureUrl);
    }

    [Fact]
    public async Task Handle_WhenPictureUrlIsUpdated_ShouldUpdateAttendee()
    {
        // Arrange
        var attendee = Factories.Attendee.CreateAttendee();
        var pictureKey = StorageKeys.AttendeePicture(attendee.Id);
        var pictureUrl = new Uri(new Uri("https://www.storage.com/"), pictureKey);

        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(attendee);

        A.CallTo(() => storageService.UploadAsync(
                pictureKey,
                command.Picture,
                cancellationToken))
            .Returns(pictureUrl);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => attendeeRepository.UpdateAsync(attendee, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenAttendeeIsUpdated_ShouldReturnSuccess()
    {
        // Arrange
        var attendee = Factories.Attendee.CreateAttendee();
        var pictureKey = StorageKeys.AttendeePicture(attendee.Id);
        var pictureUrl = new Uri(new Uri("https://www.storage.com/"), pictureKey);

        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(attendee);

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