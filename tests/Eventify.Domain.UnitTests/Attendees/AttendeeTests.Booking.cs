using Eventify.Domain.Attendees;
using Eventify.Domain.Attendees.Errors;
using Eventify.Domain.Attendees.Events;
using Eventify.Domain.Bookings.Enums;

namespace Eventify.Domain.UnitTests.Attendees;

[TestSubject(typeof(Attendee))]
public sealed partial class AttendeeTests
{
    [Fact]
    public void AddBooking_WhenBookingAlreadyExists_ShouldReturnBookingAlreadyAdded()
    {
        // Arrange
        var sut = Factories.Attendee.CreateAttendee();
        var booking = Factories.Booking.CreateBooking();

        sut.AddBooking(booking);

        // Act
        var result = sut.AddBooking(booking);

        // Assert
        result.Should().BeError(AttendeeErrors.BookingAlreadyAdded(booking.Id));
    }

    [Fact]
    public void AddBooking_WhenBookingDoesNotExist_ShouldAddBooking()
    {
        // Arrange
        var sut = Factories.Attendee.CreateAttendee();
        var booking = Factories.Booking.CreateBooking();

        // Act
        sut.AddBooking(booking);

        // Assert
        sut.BookingIds.Should().Contain(booking.Id);
    }

    [Fact]
    public void AddBooking_WhenBookingDoesNotExist_ShouldReturnSuccess()
    {
        // Arrange
        var sut = Factories.Attendee.CreateAttendee();
        var booking = Factories.Booking.CreateBooking();

        // Act
        var result = sut.AddBooking(booking);

        // Assert
        result.Should().BeValue(Result.Success);
    }

    [Fact]
    public void RequestBookingCancellation_WhenBookingDoesNotExist_ShouldReturnBookingNotFound()
    {
        // Arrange
        var sut = Factories.Attendee.CreateAttendee();
        var booking = Factories.Booking.CreateBooking();

        // Act
        var result = sut.RequestBookingCancellation(booking);

        // Assert
        result.Should().BeError(AttendeeErrors.BookingNotFound(booking.Id));
    }

    [Fact]
    public void RequestBookingCancellation_WhenBookingIsAlreadyCancelled_ShouldReturnBookingAlreadyCancelled()
    {
        // Arrange
        var sut = Factories.Attendee.CreateAttendee();
        var booking = Factories.Booking.CreateBooking();

        sut.AddBooking(booking);

        booking.Cancel(CancellationReason.EventCancelled);

        // Act
        var result = sut.RequestBookingCancellation(booking);

        // Assert
        result.Should().BeError(AttendeeErrors.BookingAlreadyCancelled(booking.Id));
    }

    [Fact]
    public void RequestBookingCancellation_WhenAnActiveBookingExists_ShouldRaiseBookingCancellationRequested()
    {
        // Arrange
        var sut = Factories.Attendee.CreateAttendee();
        var booking = Factories.Booking.CreateBooking();

        sut.AddBooking(booking);

        // Act
        sut.RequestBookingCancellation(booking);

        // Assert
        sut.DomainEvents.Should().ContainSingle(x => x is BookingCancellationRequested);
    }

    [Fact]
    public void RequestBookingCancellation_WhenBookingCancellationIsRequested_ShouldReturnSuccess()
    {
        // Arrange
        var sut = Factories.Attendee.CreateAttendee();
        var booking = Factories.Booking.CreateBooking();

        sut.AddBooking(booking);

        // Act
        var result = sut.RequestBookingCancellation(booking);

        // Assert
        result.Should().BeValue(Result.Success);
    }
}