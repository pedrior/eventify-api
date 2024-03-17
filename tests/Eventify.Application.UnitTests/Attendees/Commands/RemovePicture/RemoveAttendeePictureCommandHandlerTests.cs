using Eventify.Application.Attendees.Commands.RemovePicture;
using Eventify.Application.Attendees.Common.Errors;
using Eventify.Application.Common.Abstractions.Storage;
using Eventify.Domain.Attendees;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.UnitTests.Attendees.Commands.RemovePicture;

[TestSubject(typeof(RemoveAttendeePictureCommandHandler))]
public sealed class RemoveAttendeePictureCommandHandlerTests
{
    private readonly IUser user = A.Fake<IUser>();
    private readonly IAttendeeRepository attendeeRepository = A.Fake<IAttendeeRepository>();
    private readonly IStorageService storageService = A.Fake<IStorageService>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly RemoveAttendeePictureCommand command = new();

    private readonly RemoveAttendeePictureCommandHandler sut;

    public RemoveAttendeePictureCommandHandlerTests()
    {
        sut = new RemoveAttendeePictureCommandHandler(user, attendeeRepository, storageService);

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
    public async Task Handle_WhenAttendeeDoesNotHavePicture_ShouldReturnDeleted()
    {
        // Arrange
        var attendee = Factories.Attendee.CreateAttendee();

        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(attendee);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeValue(Result.Deleted);

        A.CallTo(() => storageService.DeleteAsync(A<Uri>._, cancellationToken))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task Handle_WhenAttendeeHasPicture_ShouldDeletePicture()
    {
        // Arrange
        var attendee = Factories.Attendee.CreateAttendee(
            pictureUrl: new Uri("https://www.some-attendee-picture.com/picture"));

        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(attendee);

        A.CallTo(() => storageService.DeleteAsync(StorageKeys.AttendeePicture(attendee.Id), cancellationToken))
            .Returns(true);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        attendee.PictureUrl.Should().BeNull();

        A.CallTo(() => storageService.DeleteAsync(StorageKeys.AttendeePicture(attendee.Id), cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenAttendeePictureDeletionFails_ShouldReturnPictureDeletionFailed()
    {
        // Arrange
        var attendee = Factories.Attendee.CreateAttendee(
            pictureUrl: new Uri("https://www.some-attendee-picture.com/picture"));

        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(attendee);

        A.CallTo(() => storageService.DeleteAsync(StorageKeys.AttendeePicture(attendee.Id), cancellationToken))
            .Returns(false);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeError(AttendeeErrors.PictureDeletionFailed);
    }

    [Fact]
    public async Task Handle_WhenAttendeePictureIsDeleted_ShouldUpdateAttendee()
    {
        // Arrange
        var attendee = Factories.Attendee.CreateAttendee(
            pictureUrl: new Uri("https://www.some-attendee-picture.com/picture"));

        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(attendee);

        A.CallTo(() => storageService.DeleteAsync(StorageKeys.AttendeePicture(attendee.Id), cancellationToken))
            .Returns(true);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => attendeeRepository.UpdateAsync(attendee, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }
}