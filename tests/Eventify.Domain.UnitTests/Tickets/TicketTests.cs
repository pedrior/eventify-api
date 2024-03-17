using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Errors;
using Eventify.Domain.Tickets.Events;

namespace Eventify.Domain.UnitTests.Tickets;

[TestSubject(typeof(Ticket))]
public sealed class TicketTests
{
    [Fact]
    public void Create_WhenQuantityPerSaleIsZero_ShouldReturnQuantityPerSaleMustBeAtLeastOne()
    {
        // Arrange
        // Act
        var result = Factories.Ticket.CreateTicket(quantityPerSale: Quantity.Zero);

        // Assert
        result.Should().BeError(TicketErrors.QuantityPerSaleMustBeAtLeastOne);
    }

    [Fact]
    public void Create_WhenCreated_ShouldRaiseTicketCreated()
    {
        // Arrange
        // Act
        var sut = Factories.Ticket.CreateTicketValue();

        // Assert
        sut.DomainEvents.Should().ContainSingle(e => e is TicketCreated);
    }

    [Fact]
    public void Update_WhenQuantityPerSaleIsZero_ShouldReturnQuantityPerSaleMustBeAtLeastOne()
    {
        // Arrange
        var sut = Factories.Ticket.CreateTicketValue();

        // Act
        var result = sut.Update(
            name: Constants.Ticket.Name,
            price: Constants.Ticket.Price,
            quantity: Constants.Ticket.Quantity,
            quantityPerSale: Quantity.Zero);

        // Assert
        result.Should().BeError(TicketErrors.QuantityPerSaleMustBeAtLeastOne);
    }

    [Fact]
    public void Update_WhenQuantityIsLessThanQuantitySold_ShouldReturnQuantityMustBeGreaterThanQuantitySold()
    {
        // Arrange
        var sut = Factories.Ticket.CreateTicketValue();

        sut.Sell();

        // Act
        var result = sut.Update(
            name: Constants.Ticket.Name,
            price: Constants.Ticket.Price,
            quantity: Quantity.Zero,
            quantityPerSale: Constants.Ticket.QuantityPerSale);

        // Assert
        result.Should().BeError(TicketErrors.QuantityMustBeGreaterThanQuantitySold(sut.QuantitySold));
    }

    [Fact]
    public void Update_WhenValid_ShouldUpdateTicket()
    {
        // Arrange
        var sut = Factories.Ticket.CreateTicketValue();

        // Act
        sut.Update(
            name: "New Name",
            price: new Money(33m),
            quantity: Constants.Ticket.Quantity,
            quantityPerSale: Constants.Ticket.QuantityPerSale,
            saleStart: DateTimeOffset.UtcNow,
            saleEnd: DateTimeOffset.UtcNow.AddDays(6),
            description: "New Description");

        // Assert
        sut.Name.Should().Be("New Name");
        sut.Price.Should().Be(new Money(33m));
        sut.Quantity.Should().Be(Constants.Ticket.Quantity);
        sut.QuantityPerSale.Should().Be(Constants.Ticket.QuantityPerSale);
        sut.SaleStart.Should().NotBeNull();
        sut.SaleEnd.Should().NotBeNull();
        sut.Description.Should().Be("New Description");
    }

    [Fact]
    public void Update_WhenUpdated_ShouldReturnUpdated()
    {
        // Arrange
        var sut = Factories.Ticket.CreateTicketValue();

        // Act
        var result = sut.Update(
            name: Constants.Ticket.Name,
            price: Constants.Ticket.Price,
            quantity: Constants.Ticket.Quantity,
            quantityPerSale: Constants.Ticket.QuantityPerSale);

        // Assert
        result.Should().BeValue(Result.Updated);
    }

    [Fact]
    public void IsAvailable_WhenIsSoldOut_ShouldReturnFalse()
    {
        // Arrange
        var sut = Factories.Ticket.CreateTicketValue(quantity: Quantity.Zero);

        // Act
        var result = sut.IsAvailable();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsAvailable_WhenIsNotInSalePeriod_ShouldReturnFalse()
    {
        // Arrange
        var sut = Factories.Ticket.CreateTicketValue(saleStart: DateTimeOffset.UtcNow.AddDays(1));

        // Act
        var result = sut.IsAvailable();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsAvailable_WhenIsNotSoldOutAndIsInSalePeriod_ShouldReturnTrue()
    {
        // Arrange
        var sut = Factories.Ticket.CreateTicketValue();

        // Act
        var result = sut.IsAvailable();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Sell_WhenIsSoldOut_ShouldReturnSoldOut()
    {
        // Arrange
        var sut = Factories.Ticket.CreateTicketValue(quantity: Quantity.Zero);

        // Act
        var result = sut.Sell();

        // Assert
        result.Should().BeError(TicketErrors.SoldOut);
    }

    [Fact]
    public void Sell_WhenIsNotInSalePeriod_ShouldReturnNotInSalePeriod()
    {
        // Arrange
        var sut = Factories.Ticket.CreateTicketValue(saleStart: DateTimeOffset.UtcNow.AddDays(1));

        // Act
        var result = sut.Sell();

        // Assert
        result.Should().BeError(TicketErrors.NotInSalePeriod(sut.SaleStart, sut.SaleEnd));
    }

    [Fact]
    public void Sell_WhenIsAvailable_ShouldIncreaseQuantitySold()
    {
        // Arrange
        var sut = Factories.Ticket.CreateTicketValue();

        // Act
        sut.Sell();

        // Assert
        sut.QuantitySold.Should().Be(Constants.Ticket.QuantityPerSale);
    }

    [Fact]
    public void Sell_WhenIsAvailable_ShouldReturnSuccess()
    {
        // Arrange
        var sut = Factories.Ticket.CreateTicketValue();

        // Act
        var result = sut.Sell();

        // Assert
        result.Should().BeValue(Result.Success);
    }

    [Fact]
    public void CancelSale_WhenQuantitySoldIsZero_ShouldReturnNoSaleToCancel()
    {
        // Arrange
        var sut = Factories.Ticket.CreateTicketValue();

        // Act
        var result = sut.CancelSale();

        // Assert
        result.Should().BeError(TicketErrors.NoSaleToCancel);
    }

    [Fact]
    public void CancelSale_WhenQuantitySoldIsNotZero_ShouldDecreaseQuantitySold()
    {
        // Arrange
        var sut = Factories.Ticket.CreateTicketValue();
        sut.Sell();

        // Act
        sut.CancelSale();

        // Assert
        sut.QuantitySold.Should().Be(Quantity.Zero);
    }

    [Fact]
    public void CancelSale_WhenQuantitySoldIsNotZero_ShouldReturnSuccess()
    {
        // Arrange
        var sut = Factories.Ticket.CreateTicketValue();
        sut.Sell();

        // Act
        var result = sut.CancelSale();

        // Assert
        result.Should().BeValue(Result.Success);
    }
}