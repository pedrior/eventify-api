using Eventify.Domain.Attendees;
using Eventify.Domain.Bookings.Errors;
using Eventify.Domain.Bookings.Services;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events;
using Eventify.Domain.Tickets;

namespace Eventify.Domain.UnitTests.Bookings.Services;

[TestSubject(typeof(BookingService))]
public sealed class BookingServiceTests
{
    private readonly Event @event = Factories.Event.CreateEvent();

    private readonly Ticket ticket = Factories.Ticket.CreateTicketValue(
        quantity: new Quantity(2),
        quantityPerSale: new Quantity(2));

    private readonly Attendee attendee = Factories.Attendee.CreateAttendee();

    private readonly BookingService sut = new();

    public BookingServiceTests()
    {
        @event.AddTicket(ticket);
    }

    [Fact]
    public void PlaceBooking_WhenEventIsNotPublished_ShouldReturnRequiredPublishedEvent()
    {
        // Arrange
        // Act
        var result = sut.PlaceBooking(@event, ticket, attendee, existingTicketBookings: []);

        // Assert
        result.Should().BeError(BookingErrors.RequiredPublishedEvent(@event.Id));
    }

    [Fact]
    public void PlaceBooking_WhenTicketIsNotAvailable_ShouldReturnUnavailableTicket()
    {
        // Arrange
        @event.Publish();
        ticket.Sell();

        // Act
        var result = sut.PlaceBooking(@event, ticket, attendee, existingTicketBookings: []);

        // Assert
        result.Should().BeError(BookingErrors.UnavailableTicket(ticket.Id));
    }

    [Fact]
    public void PlaceBooking_WhenAttendeeHasActiveBookingForTicket_ShouldReturnMultipleActiveBooking()
    {
        // Arrange
        @event.Publish();

        var booking = Factories.Booking.CreateBooking();

        // Act
        var result = sut.PlaceBooking(@event, ticket, attendee, [booking]);

        // Assert
        result.Should().BeError(BookingErrors.MultipleActiveBooking(booking.Id, ticket.Id));
    }

    [Fact]
    public void PlaceBooking_WhenBookingIsValid_ShouldReturnBooking()
    {
        // Arrange
        @event.Publish();

        // Act
        var result = sut.PlaceBooking(@event, ticket, attendee, existingTicketBookings: []);

        // Assert
        result.Should().BeValue()
            .Which.Value.Should().BeEquivalentTo(new
            {
                AttendeeId = attendee.Id,
                TicketId = ticket.Id,
                EventId = @event.Id,
                TotalPrice = ticket.Price * ticket.QuantityPerSale,
                TotalQuantity = ticket.QuantityPerSale
            });
    }
}