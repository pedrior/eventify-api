using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Errors;
using Eventify.Domain.Events.Events;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Domain.UnitTests.Events;

[TestSubject(typeof(Event))]
public sealed partial class EventTests
{
    [Fact]
    public void AddTicket_WhenEventIsFinished_ShouldReturnCannotModifyFinishedEvent()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();

        sut.AddTicket(Factories.Ticket.CreateTicketValue(ticketId: TicketId.New()));
        sut.UpdatePeriod(new Period(
            start: DateTimeOffset.UtcNow.AddMilliseconds(100),
            end: DateTimeOffset.UtcNow.AddMilliseconds(100)));

        sut.Publish();

        Thread.Sleep(200);

        // Act
        var result = sut.AddTicket(Factories.Ticket.CreateTicketValue());

        // Assert
        result.Should().BeError(EventErrors.CannotModifyFinishedEvent);
    }

    [Fact]
    public void AddTicket_WhenEventTicketIsAlreadyAdded_ShouldReturnTicketAlreadyAdded()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var ticket = Factories.Ticket.CreateTicketValue();

        sut.AddTicket(ticket);

        // Act
        var result = sut.AddTicket(ticket);

        // Assert
        result.Should().BeError(EventErrors.TicketAlreadyAdded(ticket.Id));
    }

    [Fact]
    public void AddTicket_WhenTicketExceedsEventPeriod_ShouldReturnTicketSalePeriodExceedsEventPeriod()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var ticket = Factories.Ticket.CreateTicketValue(
            saleEnd: DateTimeOffset.UtcNow.AddYears(2));

        // Act
        var result = sut.AddTicket(ticket);

        // Assert
        result.Should().BeError(EventErrors.TicketSalePeriodExceedsEventPeriod);
    }

    [Fact]
    public void AddTicket_WhenEventReachedTicketLimit_ShouldReturnTicketLimitReached()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();

        for (var i = 0; i < 10; i++)
        {
            sut.AddTicket(Factories.Ticket.CreateTicketValue(ticketId: TicketId.New()));
        }

        // Act
        var result = sut.AddTicket(Factories.Ticket.CreateTicketValue());

        // Assert
        result.Should().BeError(EventErrors.TicketLimitReached(Event.TicketsLimit));
    }

    [Fact]
    public void AddTicket_WhenTicketCanBeAdded_ShouldAddTicket()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var ticket = Factories.Ticket.CreateTicketValue();

        // Act
        sut.AddTicket(ticket);

        // Assert
        sut.TicketIds.Should().Contain(ticket.Id);
    }

    [Fact]
    public void AddTicket_WhenTicketCanBeAdded_ShouldReturnSuccess()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();

        // Act
        var result = sut.AddTicket(Factories.Ticket.CreateTicketValue());

        // Assert
        result.Should().BeValue(Result.Success);
    }

    [Fact]
    public void RemoveTicket_WhenEventIsFinished_ShouldReturnCannotModifyFinishedEvent()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();

        sut.AddTicket(Factories.Ticket.CreateTicketValue(ticketId: TicketId.New()));
        sut.UpdatePeriod(new Period(
            start: DateTimeOffset.UtcNow.AddMilliseconds(100),
            end: DateTimeOffset.UtcNow.AddMilliseconds(100)));

        sut.Publish();

        Thread.Sleep(200);

        // Act
        var result = sut.RemoveTicket(Factories.Ticket.CreateTicketValue());

        // Assert
        result.Should().BeError(EventErrors.CannotModifyFinishedEvent);
    }

    [Fact]
    public void RemoveTicket_WhenTicketIsNotAdded_ShouldReturnTicketNotFound()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var ticket = Factories.Ticket.CreateTicketValue();

        // Act
        var result = sut.RemoveTicket(ticket);

        // Assert
        result.Should().BeError(EventErrors.TicketNotFound(ticket.Id));
    }

    [Fact]
    public void RemoveTicket_WhenTicketCanBeRemoved_ShouldRemoveTicket()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var ticket = Factories.Ticket.CreateTicketValue();

        sut.AddTicket(ticket);

        // Act
        sut.RemoveTicket(ticket);

        // Assert
        sut.TicketIds.Should().NotContain(ticket.Id);
    }

    [Fact]
    public void RemoveTicket_WhenTicketCanBeRemoved_ShouldRaiseTicketRemoved()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var ticket = Factories.Ticket.CreateTicketValue();

        sut.AddTicket(ticket);

        // Act
        sut.RemoveTicket(ticket);

        // Assert
        sut.DomainEvents.Should().ContainSingle(e => e is TicketRemoved);
    }

    [Fact]
    public void RemoveTicket_WhenTicketCanBeRemoved_ShouldReturnDeleted()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var ticket = Factories.Ticket.CreateTicketValue();

        sut.AddTicket(ticket);

        // Act
        var result = sut.RemoveTicket(ticket);

        // Assert
        result.Should().BeValue(Result.Deleted);
    }
}