using Eventify.Domain.Events;
using Eventify.Domain.Events.Errors;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Domain.UnitTests.Events;

[TestSubject(typeof(Event))]
public sealed partial class EventTests
{
    [Fact]
    public void AddBooking_WheEventIsNotPublished_ShouldReturnInvalidOperation()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var booking = Factories.Booking.CreateBooking();

        // Act
        var result = sut.AddBooking(booking);

        // Assert
        result.Should().BeError(EventErrors.InvalidOperation(sut.State));
    }

    [Fact]
    public void AddBooking_WhenBookingIsAlreadyAdded_ShouldReturnBookingAlreadyAdded()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var ticket = Factories.Ticket.CreateTicketValue();
        var booking = Factories.Booking.CreateBooking(ticketId: ticket.Id);

        sut.AddTicket(ticket);
        sut.Publish();
        sut.AddBooking(booking);

        // Act
        var result = sut.AddBooking(booking);

        // Assert
        result.Should().BeError(EventErrors.BookingAlreadyAdded(booking.Id));
    }

    [Fact]
    public void AddBooking_WhenEventDoesNotHaveBookingTicket_ShouldReturnTicketNotFound()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var ticket = Factories.Ticket.CreateTicketValue();
        var booking = Factories.Booking.CreateBooking(ticketId: TicketId.New());

        sut.AddTicket(ticket);
        sut.Publish();
        sut.AddBooking(booking);

        // Act
        var result = sut.AddBooking(booking);

        // Assert
        result.Should().BeError(EventErrors.TicketNotFound(booking.TicketId));
    }

    [Fact]
    public void AddBooking_WhenBookingCanBeAdded_ShouldAddBooking()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var ticket = Factories.Ticket.CreateTicketValue();
        var booking = Factories.Booking.CreateBooking(ticketId: ticket.Id);

        sut.AddTicket(ticket);
        sut.Publish();

        // Act
        sut.AddBooking(booking);

        // Assert
        sut.BookingIds.Should().Contain(booking.Id);
    }

    [Fact]
    public void AddBooking_WhenBookingCanBeAdded_ShouldReturnSuccess()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var ticket = Factories.Ticket.CreateTicketValue();
        var booking = Factories.Booking.CreateBooking(ticketId: ticket.Id);

        sut.AddTicket(ticket);
        sut.Publish();

        // Act
        var result = sut.AddBooking(booking);

        // Assert
        result.Should().BeValue(Result.Success);
    }
}