using Eventify.Application.Producers.Commands.CreateProfile;
using Eventify.Application.Producers.Common.Errors;
using Eventify.Domain.Producers;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.UnitTests.Producers.Commands.CreateProfile;

[TestSubject(typeof(CreateProducerProfileCommandHandler))]
public sealed class CreateProducerProfileCommandHandlerTests
{
    private readonly IUser user = A.Fake<IUser>();
    private readonly IProducerRepository producerRepository = A.Fake<IProducerRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly CreateProducerProfileCommand command = new()
    {
        Name = "John Doe Events Ltd.",
        Bio = "We are a corporate events company based in London.",
        Email = "johndoeevent@contact.com",
        PhoneNumber = "1234567890",
        WebsiteUrl = new Uri("https://johndoeevents.com")
    };

    private readonly CreateProducerProfileCommandHandler sut;

    public CreateProducerProfileCommandHandlerTests()
    {
        sut = new CreateProducerProfileCommandHandler(user, producerRepository);

        A.CallTo(() => user.Id)
            .Returns(Constants.UserId);
    }

    [Fact]
    public async Task Handle_WhenProducerProfileAlreadyExists_ShouldReturnDuplicateProfile()
    {
        // Arrange
        A.CallTo(() => producerRepository.ExistsByUserAsync(user.Id, cancellationToken))
            .Returns(true);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeFailure(ProducerErrors.ProfileAlreadyCreated);
    }

    [Fact]
    public async Task Handle_WhenProducerProfileDoesNotExist_ShouldCreateProducerProfile()
    {
        // Arrange
        A.CallTo(() => producerRepository.ExistsByUserAsync(user.Id, cancellationToken))
            .Returns(false);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => producerRepository.AddAsync(A<Producer>.That.Matches(p =>
                    p.UserId == user.Id &&
                    p.Details.Name == command.Name &&
                    p.Details.Bio == command.Bio &&
                    p.Contact.Email == command.Email &&
                    p.Contact.PhoneNumber == command.PhoneNumber &&
                    p.WebsiteUrl == command.WebsiteUrl),
                cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenProducerProfileIsCreated_ShouldReturnSuccess()
    {
        // Arrange
        A.CallTo(() => producerRepository.ExistsByUserAsync(user.Id, cancellationToken))
            .Returns(false);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeSuccess(Success.Value);
    }
}