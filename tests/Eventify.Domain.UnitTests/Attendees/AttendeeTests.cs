using Eventify.Domain.Attendees;
using Eventify.Domain.Attendees.Events;
using Eventify.Domain.Attendees.ValueObjects;

namespace Eventify.Domain.UnitTests.Attendees;

[TestSubject(typeof(Attendee))]
public sealed partial class AttendeeTests
{
    [Fact]
    public void Create_WhenCalled_ShouldRaiseAttendeeCreated()
    {
        // Arrange
        // Act
        var sut = Factories.Attendee.CreateAttendee();

        // Assert
        sut.DomainEvents.Should().ContainSingle(x => x is AttendeeCreated);
    }

    [Fact]
    public void UpdateProfile_WhenCalled_ShouldUpdateProfile()
    {
        // Arrange
        var sut = Factories.Attendee.CreateAttendee();
        var newDetails = new AttendeeDetails(
            givenName: "Jane",
            familyName: "Doe",
            birthDate: new DateOnly(2000, 9, 5));

        var newContact = new AttendeeContact(email: "jane@doe.com");

        // Act
        sut.UpdateProfile(newDetails, newContact);

        // Assert
        sut.Details.Should().Be(newDetails);
    }

    [Fact]
    public void SetPicture_WhenCalled_ShouldSetPictureUrl()
    {
        // Arrange
        var sut = Factories.Attendee.CreateAttendee();
        var newPictureUrl = new Uri("https://example.com/picture.jpg");

        // Act
        sut.SetPicture(newPictureUrl);

        // Assert
        sut.PictureUrl.Should().Be(newPictureUrl);
    }

    [Fact]
    public void SetPicture_WhenCalled_ShouldReturnSuccess()
    {
        // Arrange
        var sut = Factories.Attendee.CreateAttendee();
        var newPictureUrl = new Uri("https://example.com/picture.jpg");

        // Act
        var result = sut.SetPicture(newPictureUrl);

        // Assert
        result.Should().BeSuccess(Success.Value);
    }

    [Fact]
    public void RemovePicture_WhenCalled_ShouldRemovePictureUrl()
    {
        // Arrange
        var sut = Factories.Attendee.CreateAttendee();

        sut.SetPicture(new Uri("https://example.com/picture.jpg"));

        // Act
        sut.RemovePicture();

        // Assert
        sut.PictureUrl.Should().BeNull();
    }

    [Fact]
    public void RemovePicture_WhenCalled_ShouldReturnSuccess()
    {
        // Arrange
        var sut = Factories.Attendee.CreateAttendee();

        sut.SetPicture(new Uri("https://example.com/picture.jpg"));

        // Act
        var result = sut.RemovePicture();

        // Assert
        result.Should().BeSuccess(Success.Value);
    }
}