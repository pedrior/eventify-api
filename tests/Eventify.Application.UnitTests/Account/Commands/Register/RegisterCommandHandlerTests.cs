using Eventify.Application.Account.Commands.Register;
using Eventify.Application.Common.Abstractions.Identity;
using Eventify.Domain.Attendees;
using Eventify.Domain.Attendees.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.UnitTests.Account.Commands.Register;

[TestSubject(typeof(RegisterCommandHandler))]
public sealed class RegisterCommandHandlerTests
{
    private readonly IIdentityService identityService = A.Fake<IIdentityService>();
    private readonly IAttendeeRepository attendeeRepository = A.Fake<IAttendeeRepository>();
    private readonly ILogger<RegisterCommandHandler> logger = A.Dummy<ILogger<RegisterCommandHandler>>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly RegisterCommand command = new()
    {
        Email = "john@doe.com",
        Password = "john1234",
        GivenName = "John",
        FamilyName = "Doe",
        PhoneNumber = "+559999999999",
        BirthDate = new DateOnly(2000, 9, 5)
    };

    private readonly RegisterCommandHandler sut;

    public RegisterCommandHandlerTests()
    {
        sut = new RegisterCommandHandler(identityService, attendeeRepository, logger);
    }

    [Fact]
    public async Task Handle_WhenRegistrationIsSuccessful_ShouldReturnCreated()
    {
        // Arrange
        A.CallTo(() => identityService.CreateUserAsync(command.Email, command.Password))
            .Returns("user-id");

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeValue(Result.Created);
    }

    [Fact]
    public async Task Handle_WhenRegistrationIsUnsuccessful_ShouldReturnError()
    {
        // Arrange
        var error = Error.Conflict();

        A.CallTo(() => identityService.CreateUserAsync(command.Email, command.Password))
            .Returns(error);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeError(error);
    }

    [Fact]
    public async Task Handle_WhenRegistrationIsSuccessful_ShouldCreateAttendee()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();

        A.CallTo(() => identityService.CreateUserAsync(command.Email, command.Password))
            .Returns(userId);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => attendeeRepository.AddAsync(A<Attendee>.That.Matches(attendee =>
                attendee.UserId == userId &&
                attendee.Details.GivenName == command.GivenName &&
                attendee.Details.FamilyName == command.FamilyName &&
                attendee.Details.BirthDate == command.BirthDate &&
                attendee.Contact.Email == command.Email &&
                attendee.Contact.PhoneNumber == command.PhoneNumber), cancellationToken))
            .MustHaveHappenedOnceExactly();
    }
}