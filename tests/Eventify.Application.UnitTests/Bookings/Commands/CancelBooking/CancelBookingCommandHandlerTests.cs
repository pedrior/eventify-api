using Eventify.Application.Bookings.Commands.CancelBooking;
using Eventify.Application.Bookings.Common.Errors;
using Eventify.Domain.Attendees;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Users;
using Eventify.Domain.Users.ValueObjects;

namespace Eventify.Application.UnitTests.Bookings.Commands.CancelBooking;

[TestSubject(typeof(CancelBookingCommandHandler))]
public sealed class CancelBookingCommandHandlerTests
{
    private readonly IUser user = A.Fake<IUser>();
    private readonly IAttendeeRepository attendeeRepository = A.Fake<IAttendeeRepository>();
    private readonly IBookingRepository bookingRepository = A.Fake<IBookingRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly CancelBookingCommand command = new()
    {
        BookingId = Guid.NewGuid()
    };

    private readonly UserId userId = Constants.UserId;
    
    private readonly Attendee attendee = Factories.Attendee.CreateAttendee();
    private readonly Booking booking = Factories.Booking.CreateBooking();
    
    private readonly CancelBookingCommandHandler sut;

    public CancelBookingCommandHandlerTests()
    {
        sut = new CancelBookingCommandHandler(user, attendeeRepository, bookingRepository);

        A.CallTo(() => user.Id)
            .Returns(userId);
        
        A.CallTo(() => bookingRepository.GetAsync(command.BookingId, cancellationToken))
            .Returns(booking);

        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(attendee);
    }

    [Fact]
    public async Task Handle_WhenBookingDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        A.CallTo(() => bookingRepository.GetAsync(command.BookingId, cancellationToken))
            .Returns(null as Booking);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeFailure(BookingErrors.NotFound(command.BookingId));
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
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage("Attendee not found");
    }

    [Fact]
    public async Task Handle_WhenRequestBookingCancellationFails_ShouldReturnError()
    {
        // Arrange
        // Act
        // The attendee doesn't have the booking to cancel
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeFailure();
    }

    [Fact]
    public async Task Handle_WhenRequestBookingCancellationSucceeds_ShouldUpdateAttendee()
    {
        // Arrange
        attendee.AddBooking(booking);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => attendeeRepository.UpdateAsync(attendee, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenRequestBookingCancellationSucceeds_ShouldReturnSuccess()
    {
        // Arrange
        attendee.AddBooking(booking);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeSuccess(Success.Value);
    }
}