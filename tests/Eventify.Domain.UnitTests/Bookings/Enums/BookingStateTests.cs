using Eventify.Domain.Bookings.Enums;

namespace Eventify.Domain.UnitTests.Bookings.Enums;

[TestSubject(typeof(BookingState))]
public sealed class BookingStateTests
{
    public static IEnumerable<object[]> ActiveBookingState()
    {
        yield return [BookingState.Pending, true];
        yield return [BookingState.Paid, true];
        yield return [BookingState.Confirmed, true];
        yield return [BookingState.Cancelled, false];
    }

    public static IEnumerable<object[]> TransitionToNewState()
    {
        yield return [BookingState.Pending, BookingState.Pending, false];
        yield return [BookingState.Pending, BookingState.Paid, true];
        yield return [BookingState.Pending, BookingState.Confirmed, true];
        yield return [BookingState.Pending, BookingState.Cancelled, true];
        yield return [BookingState.Paid, BookingState.Pending, false];
        yield return [BookingState.Paid, BookingState.Paid, false];
        yield return [BookingState.Paid, BookingState.Confirmed, true];
        yield return [BookingState.Paid, BookingState.Cancelled, true];
        yield return [BookingState.Confirmed, BookingState.Pending, false];
        yield return [BookingState.Confirmed, BookingState.Paid, false];
        yield return [BookingState.Confirmed, BookingState.Confirmed, false];
        yield return [BookingState.Confirmed, BookingState.Cancelled, true];
        yield return [BookingState.Cancelled, BookingState.Pending, false];
        yield return [BookingState.Cancelled, BookingState.Paid, false];
        yield return [BookingState.Cancelled, BookingState.Confirmed, false];
        yield return [BookingState.Cancelled, BookingState.Cancelled, false];
    }

    [Theory, MemberData(nameof(ActiveBookingState))]
    public void IsActive_WhenCalled_ReturnsExpectedResult(BookingState state, bool expected)
    {
        // Arrange
        // Act
        var result = state.IsActive();

        // Assert
        result.Should().Be(expected);
    }

    [Theory, MemberData(nameof(TransitionToNewState))]
    public void CanTransitionTo_WhenCalled_ReturnsExpectedResult(BookingState current, BookingState @new, bool expected)
    {
        // Arrange
        // Act
        var result = current.CanTransitionTo(@new);

        // Assert
        result.Should().Be(expected);
    }
}