using Eventify.Application.Bookings.Events;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Enums;
using Eventify.Domain.Bookings.Events;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.UnitTests.Bookings.Events;

[TestSubject(typeof(BookingCancelledHandler))]
public sealed class BookingCancelledHandlerTests
{
    private readonly ITicketRepository ticketRepository = A.Fake<ITicketRepository>();
    private readonly ILogger<BookingCancelledHandler> logger = A.Fake<ILogger<BookingCancelledHandler>>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly BookingCancelledHandler sut;

    private readonly Booking booking = Factories.Booking.CreateBooking();
    private readonly Ticket ticket = Factories.Ticket.CreateTicketValue();

    private readonly BookingCancelled e;

    public BookingCancelledHandlerTests()
    {
        sut = new BookingCancelledHandler(ticketRepository, logger);

        e = new BookingCancelled(booking, CancellationReason.AttendeeRequest);
        
        A.CallTo(() => ticketRepository.GetAsync(booking.TicketId, cancellationToken))
            .Returns(ticket);
    }

    [Fact]
    public async Task Handle_WhenCancellationReasonIsPaymentFailed_ShouldNotProcessTicketSaleCancellation()
    {
        // Arrange
        // Act
        await sut.Handle(e with
        {
            Reason = CancellationReason.PaymentFailed
        }, cancellationToken);

        // Assert
        A.CallTo(() => ticketRepository.GetAsync(booking.TicketId, cancellationToken))
            .MustNotHaveHappened();
    }

    [Fact]
    public async Task Handle_WhenTicketDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => ticketRepository.GetAsync(booking.TicketId, cancellationToken))
            .Returns(null as Ticket);

        // Act
        var act = () => sut.Handle(e, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage($"Ticket {booking.TicketId} not found");
    }

    [Fact]
    public async Task Handle_WhenTicketCancellationFails_ShouldThrowResultException()
    {
        // Arrange
        var soldOutTicket = Factories.Ticket.CreateTicketValue(quantity: Quantity.Zero);

        A.CallTo(() => ticketRepository.GetAsync(booking.TicketId, cancellationToken))
            .Returns(soldOutTicket);

        // Act
        var act = () => sut.Handle(e, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ResultException>();
    }

    [Fact]
    public async Task Handle_WhenTicketCancellationSucceeds_ShouldUpdateTicket()
    {
        // Arrange
        ticket.Sell();
        
        // Act
        await sut.Handle(e, cancellationToken);

        // Assert
        A.CallTo(() => ticketRepository.UpdateAsync(ticket, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }
}