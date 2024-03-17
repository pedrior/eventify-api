using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Domain.UnitTests.Common.ValueObjects;

[TestSubject(typeof(Quantity))]
public sealed class QuantityTests
{
    [Fact]
    public void New_WhenValueIsNegative_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        // Act
        var act = () => new Quantity(-1);

        // Assert
        act.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }
}