using Eventify.Application.Bookings.Events;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Enums;
using Eventify.Domain.Bookings.Events;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.UnitTests.Bookings.Events;

[TestSubject(typeof(BookingPaidHandler))]
public sealed class BookingPaidHandlerTests
{
    private readonly IBookingRepository bookingRepository = A.Fake<IBookingRepository>();
    private readonly ITicketRepository ticketRepository = A.Fake<ITicketRepository>();
    private readonly ILogger<BookingPaidHandler> logger = A.Fake<ILogger<BookingPaidHandler>>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly BookingPaidHandler sut;

    private readonly Booking booking = Factories.Booking.CreateBooking();
    private readonly Ticket ticket = Factories.Ticket.CreateTicketValue();

    private readonly BookingPaid e;

    public BookingPaidHandlerTests()
    {
        sut = new BookingPaidHandler(bookingRepository, ticketRepository, logger);

        e = new BookingPaid(booking);

        A.CallTo(() => bookingRepository.GetAsync(e.Booking.Id, cancellationToken))
            .Returns(booking);

        A.CallTo(() => ticketRepository.GetAsync(e.Booking.TicketId, cancellationToken))
            .Returns(ticket);
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldConfirmBooking()
    {
        // Arrange
        // Act
        await sut.Handle(e, cancellationToken);

        // Assert
        booking.State.Should().Be(BookingState.Confirmed);

        A.CallTo(() => bookingRepository.UpdateAsync(booking, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenBookedTicketDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => ticketRepository.GetAsync(e.Booking.TicketId, cancellationToken))
            .Returns(null as Ticket);

        // Act
        var act = () => sut.Handle(e, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage($"Ticket {booking.TicketId} not found");
    }

    [Fact]
    public async Task Handle_WhenTicketSellFails_ShouldCancelBooking()
    {
        // Arrange
        var soldOutTicket = Factories.Ticket.CreateTicketValue(quantity: Quantity.Zero);

        A.CallTo(() => ticketRepository.GetAsync(e.Booking.TicketId, cancellationToken))
            .Returns(soldOutTicket);

        // Act
        await sut.Handle(e, cancellationToken);

        // Assert
        booking.State.Should().Be(BookingState.Cancelled);

        A.CallTo(() => bookingRepository.UpdateAsync(booking, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenBookingConfirmFails_ShouldCancelBooking()
    {
        // Arrange
        var alreadyConfirmedBooking = Factories.Booking.CreateBooking();
        alreadyConfirmedBooking.Confirm();

        // Act
        await sut.Handle(new BookingPaid(alreadyConfirmedBooking), cancellationToken);

        // Assert
        alreadyConfirmedBooking.State.Should().Be(BookingState.Cancelled);

        A.CallTo(() => bookingRepository.UpdateAsync(alreadyConfirmedBooking, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }
    
    [Fact]
    public async Task Handle_WhenCancelFailedBookingFails_ShouldThrowResultException()
    {
        // Arrange
        var alreadyConfirmedBooking = Factories.Booking.CreateBooking();
        alreadyConfirmedBooking.Confirm();
        alreadyConfirmedBooking.Cancel(CancellationReason.TicketUnavailable);
        
        // Act
        var act = () => sut.Handle(new BookingPaid(alreadyConfirmedBooking), cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ResultException>();
    }
}