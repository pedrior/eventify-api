using Eventify.Domain.Producers;
using Eventify.Domain.Producers.Events;
using Eventify.Domain.Producers.ValueObjects;

namespace Eventify.Domain.UnitTests.Producers;

[TestSubject(typeof(Producer))]
public sealed partial class ProducerTests
{
    [Fact]
    public void Create_WhenCalled_ShouldRaiseProducerCreated()
    {
        // Arrange
        // Act
        var sut = Factories.Producer.CreateProducer();

        // Assert
        sut.DomainEvents.Should().ContainSingle(x => x is ProducerCreated);
    }

    [Fact]
    public void UpdateProfile_WhenCalled_ShouldUpdateProfile()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var newDetails = new ProducerDetails(name: "Jane & John Doe Ltd.");

        var newContact = new ProducerContact(
            email: "jane@doe.com",
            phoneNumber: "+1234567890");

        // Act
        sut.UpdateProfile(newDetails, newContact);

        // Assert
        sut.Details.Should().Be(newDetails);
    }

    [Fact]
    public void SetPicture_WhenCalled_ShouldSetPictureUrl()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var newPictureUrl = new Uri("https://suts.com/picture.jpg");

        // Act
        sut.SetPicture(newPictureUrl);

        // Assert
        sut.PictureUrl.Should().Be(newPictureUrl);
    }

    [Fact]
    public void SetPicture_WhenCalled_ShouldReturnSuccess()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var newPictureUrl = new Uri("https://suts.com/picture.jpg");

        // Act
        var result = sut.SetPicture(newPictureUrl);

        // Assert
        result.Should().BeValue(Result.Success);
    }

    [Fact]
    public void RemovePicture_WhenCalled_ShouldRemovePictureUrl()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();

        sut.SetPicture(new Uri("https://suts.com/picture.jpg"));

        // Act
        sut.RemovePicture();

        // Assert
        sut.PictureUrl.Should().BeNull();
    }

    [Fact]
    public void RemovePicture_WhenCalled_ShouldReturnDeleted()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();

        sut.SetPicture(new Uri("https://suts.com/picture.jpg"));

        // Act
        var result = sut.RemovePicture();

        // Assert
        result.Should().BeValue(Result.Deleted);
    }
}