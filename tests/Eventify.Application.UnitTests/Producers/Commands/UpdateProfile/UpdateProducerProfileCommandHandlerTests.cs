using Eventify.Application.Producers.Commands.UpdateProfile;
using Eventify.Domain.Producers;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Producers.ValueObjects;
using Eventify.Domain.Users;

namespace Eventify.Application.UnitTests.Producers.Commands.UpdateProfile;

[TestSubject(typeof(UpdateProducerProfileCommandHandler))]
public sealed class UpdateProducerProfileCommandHandlerTests
{
    private readonly IUser user = A.Fake<IUser>();
    private readonly IProducerRepository producerRepository = A.Fake<IProducerRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly UpdateProducerProfileCommand command = new()
    {
        Name = "Jane & John Doe Ltd.",
        Bio = null,
        Email = "jane@doe.com",
        PhoneNumber = "+5581999999999",
        WebsiteUrl = null
    };

    private readonly UpdateProducerProfileCommandHandler sut;

    public UpdateProducerProfileCommandHandlerTests()
    {
        sut = new UpdateProducerProfileCommandHandler(user, producerRepository);

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
    public async Task Handle_WhenProducerExists_ShouldUpdateProfile()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer();

        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(producer);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        producer.Details.Should().Be(new ProducerDetails(command.Name, command.Bio));
        producer.Contact.Should().Be(new ProducerContact(producer.Contact.Email, command.PhoneNumber));
        producer.WebsiteUrl.Should().Be(command.WebsiteUrl);
    }

    [Fact]
    public async Task Handle_WhenProducerExists_ShouldUpdateProducer()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer();

        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(producer);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => producerRepository.UpdateAsync(producer, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenProducerExists_ShouldReturnSuccess()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer();

        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(producer);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeSuccess(Success.Value);
    }
}