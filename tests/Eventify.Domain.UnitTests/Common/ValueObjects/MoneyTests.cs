using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Domain.UnitTests.Common.ValueObjects;

[TestSubject(typeof(Money))]
public sealed class MoneyTests
{
    [Fact]
    public void New_WhenValueIsNegative_ShouldThrowArgumentOutOfRangeException()
    {
        // Arrange
        // Act
        var act = () => new Money(-1m);

        // Assert
        act.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void Add_WhenCalled_ShouldReturnSum()
    {
        // Arrange
        var money1 = new Money(5m);
        var money2 = new Money(7m);

        // Act
        var result = money1 + money2;

        // Assert
        result.Should().Be(new Money(12m));
    }
    
    [Fact]
    public void Subtract_WhenCalled_ShouldReturnDifference()
    {
        // Arrange
        var money1 = new Money(8.5m);
        var money2 = new Money(2.5m);

        // Act
        var result = money1 - money2;

        // Assert
        result.Should().Be(new Money(6m));
    }
}