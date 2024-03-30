using Eventify.Application.Attendees.Commands.UpdateProfile;
using Eventify.Domain.Attendees;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Attendees.ValueObjects;
using Eventify.Domain.Users;

namespace Eventify.Application.UnitTests.Attendees.Commands.UpdateProfile;

[TestSubject(typeof(UpdateAttendeeProfileCommandHandler))]
public sealed class UpdateAttendeeProfileCommandHandlerTests
{
    private readonly IUser user = A.Fake<IUser>();
    private readonly IAttendeeRepository attendeeRepository = A.Fake<IAttendeeRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly UpdateAttendeeProfileCommand command = new()
    {
        GivenName = "Pedro",
        FamilyName = "Júnior",
        PhoneNumber = "+5581999999999",
        BirthDate = new DateOnly(2000, 9, 5)
    };

    private readonly UpdateAttendeeProfileCommandHandler sut;

    public UpdateAttendeeProfileCommandHandlerTests()
    {
        sut = new UpdateAttendeeProfileCommandHandler(user, attendeeRepository);

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
    public async Task Handle_WhenAttendeeExists_ShouldUpdateProfile()
    {
        // Arrange
        var attendee = Factories.Attendee.CreateAttendee();

        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(attendee);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        attendee.Details.Should().Be(new AttendeeDetails(command.GivenName, command.FamilyName, command.BirthDate));
        attendee.Contact.Should().Be(new AttendeeContact(attendee.Contact.Email, command.PhoneNumber));
    }

    [Fact]
    public async Task Handle_WhenAttendeeExists_ShouldUpdateAttendee()
    {
        // Arrange
        var attendee = Factories.Attendee.CreateAttendee();

        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(attendee);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => attendeeRepository.UpdateAsync(attendee, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenAttendeeExists_ShouldReturnSuccess()
    {
        // Arrange
        var attendee = Factories.Attendee.CreateAttendee();

        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(attendee);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeSuccess(Success.Value);
    }
}