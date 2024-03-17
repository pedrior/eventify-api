using Eventify.Domain.Events.Enums;

namespace Eventify.Domain.UnitTests.Events.Enums;

[TestSubject(typeof(EventState))]
public sealed class EventStateTests
{
    public static IEnumerable<object[]> TransitionToNewState()
    {
        yield return [EventState.Draft, EventState.Draft, false];
        yield return [EventState.Draft, EventState.Published, true];
        yield return [EventState.Published, EventState.Draft, true];
        yield return [EventState.Published, EventState.Published, false];
    }
    
    [Theory, MemberData(nameof(TransitionToNewState))]
    public void CanTransitionTo_WhenCalled_ReturnsExpectedResult(EventState current, EventState @new, bool expected)
    {
        // Arrange
        // Act
        var result = current.CanTransitionTo(@new);

        // Assert
        result.Should().Be(expected);
    }
}