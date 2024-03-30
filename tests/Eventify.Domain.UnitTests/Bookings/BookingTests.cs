using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Enums;
using Eventify.Domain.Bookings.Errors;
using Eventify.Domain.Bookings.Events;

namespace Eventify.Domain.UnitTests.Bookings;

[TestSubject(typeof(Booking))]
public sealed class BookingTests
{
    [Fact]
    public void Create_WhenCalled_ShouldCreateBookingWithPendingState()
    {
        // Arrange
        // Act
        var sut = Factories.Booking.CreateBooking();

        // Assert
        sut.State.Should().Be(BookingState.Pending);
    }

    [Fact]
    public void Create_WhenCalled_ShouldCreateBookingWithPlacedAtCloseToNow()
    {
        // Arrange
        // Act
        var sut = Factories.Booking.CreateBooking();

        // Assert
        sut.PlacedAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Create_WhenCalled_ShouldRaiseBookingPlaced()
    {
        // Arrange
        // Act
        var sut = Factories.Booking.CreateBooking();

        // Assert
        sut.DomainEvents.Should().ContainSingle(e => e is BookingPlaced);
    }

    [Fact]
    public void Pay_WhenTransitionIsValid_ShouldSetStateToPaid()
    {
        // Arrange
        var sut = Factories.Booking.CreateBooking();

        // Act
        sut.Pay();

        // Assert
        sut.State.Should().Be(BookingState.Paid);
    }

    [Fact]
    public void Pay_WhenTransitionIsValid_ShouldSetPaidAtCloseToNow()
    {
        // Arrange
        var sut = Factories.Booking.CreateBooking();

        // Act
        sut.Pay();

        // Assert
        sut.PaidAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Pay_WhenTransitionIsValid_ShouldRaiseBookingPaid()
    {
        // Arrange
        var sut = Factories.Booking.CreateBooking();

        // Act
        sut.Pay();

        // Assert
        sut.DomainEvents.Should().ContainSingle(e => e is BookingPaid);
    }

    [Fact]
    public void Pay_WhenTransitionIsInvalid_ShouldReturnInvalidOperation()
    {
        // Arrange
        var sut = Factories.Booking.CreateBooking();
        sut.Pay();

        // Act
        var result = sut.Pay();

        // Assert
        result.Should().BeFailure(BookingErrors.InvalidStateOperation(sut.State));
    }
    
    [Fact]
    public void Cancel_WhenTransitionIsValid_ShouldSetStateToCancelled()
    {
        // Arrange
        var sut = Factories.Booking.CreateBooking();

        // Act
        sut.Cancel(CancellationReason.AttendeeRequest);

        // Assert
        sut.State.Should().Be(BookingState.Cancelled);
    }
    
    [Fact]
    public void Cancel_WhenTransitionIsValid_ShouldSetCancelledAtCloseToNow()
    {
        // Arrange
        var sut = Factories.Booking.CreateBooking();

        // Act
        sut.Cancel(CancellationReason.AttendeeRequest);

        // Assert
        sut.CancelledAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));
    }
    
    [Fact]
    public void Cancel_WhenTransitionIsValid_ShouldSetCancellationReason()
    {
        // Arrange
        var sut = Factories.Booking.CreateBooking();

        // Act
        sut.Cancel(CancellationReason.AttendeeRequest);

        // Assert
        sut.CancellationReason.Should().Be(CancellationReason.AttendeeRequest);
    }
    
    [Fact]
    public void Cancel_WhenTransitionIsValid_ShouldRaiseBookingCancelled()
    {
        // Arrange
        var sut = Factories.Booking.CreateBooking();

        // Act
        sut.Cancel(CancellationReason.AttendeeRequest);

        // Assert
        sut.DomainEvents.Should().ContainSingle(e => e is BookingCancelled);
    }
    
    [Fact]
    public void Cancel_WhenTransitionIsInvalid_ShouldReturnInvalidOperation()
    {
        // Arrange
        var sut = Factories.Booking.CreateBooking();
        sut.Cancel(CancellationReason.AttendeeRequest);

        // Act
        var result = sut.Cancel(CancellationReason.AttendeeRequest);

        // Assert
        result.Should().BeFailure(BookingErrors.InvalidStateOperation(sut.State));
    }
    
    [Fact]
    public void Confirm_WhenTransitionIsValid_ShouldSetStateToConfirmed()
    {
        // Arrange
        var sut = Factories.Booking.CreateBooking();

        // Act
        sut.Confirm();

        // Assert
        sut.State.Should().Be(BookingState.Confirmed);
    }
    
    [Fact]
    public void Confirm_WhenTransitionIsValid_ShouldSetConfirmedAtCloseToNow()
    {
        // Arrange
        var sut = Factories.Booking.CreateBooking();

        // Act
        sut.Confirm();

        // Assert
        sut.ConfirmedAt.Should().BeCloseTo(DateTimeOffset.Now, TimeSpan.FromSeconds(1));
    }
    
    [Fact]
    public void Confirm_WhenTransitionIsValid_ShouldRaiseBookingConfirmed()
    {
        // Arrange
        var sut = Factories.Booking.CreateBooking();

        // Act
        sut.Confirm();

        // Assert
        sut.DomainEvents.Should().ContainSingle(e => e is BookingConfirmed);
    }
    
    [Fact]
    public void Confirm_WhenTransitionIsInvalid_ShouldReturnInvalidOperation()
    {
        // Arrange
        var sut = Factories.Booking.CreateBooking();
        sut.Confirm();

        // Act
        var result = sut.Confirm();

        // Assert
        result.Should().BeFailure(BookingErrors.InvalidStateOperation(sut.State));
    }
}