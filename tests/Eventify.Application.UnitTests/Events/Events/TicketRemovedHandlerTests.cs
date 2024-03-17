using Eventify.Application.Events.Events;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Events.Events;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.UnitTests.Events.Events;

[TestSubject(typeof(TicketRemovedHandler))]
public sealed class TicketRemovedHandlerTests
{
    private readonly IBookingRepository bookingRepository = A.Fake<IBookingRepository>();
    private readonly ITicketRepository ticketRepository = A.Fake<ITicketRepository>();
    private readonly ILogger<TicketRemovedHandler> logger = A.Fake<ILogger<TicketRemovedHandler>>();
    
    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly Ticket ticket = Factories.Ticket.CreateTicketValue();

    private readonly TicketRemoved e;
    private readonly TicketRemovedHandler sut;

    public TicketRemovedHandlerTests()
    {
        e = new TicketRemoved(ticket);
        sut = new TicketRemovedHandler(bookingRepository, ticketRepository, logger);
    }
    
    [Fact]
    public async Task Handle_WhenCalled_ShouldCancelAllEventBookingsAsync()
    {
        // Arrange
        A.CallTo(() => bookingRepository.ListByTicketAsync(e.Ticket.Id, cancellationToken))
            .Returns(
            [
                Factories.Booking.CreateBooking(),
                Factories.Booking.CreateBooking(),
                Factories.Booking.CreateBooking()
            ]);
        
        // Act
        await sut.Handle(e, cancellationToken);

        // Assert
        A.CallTo(() => bookingRepository.UpdateAsync(A<Booking>._, cancellationToken))
            .MustHaveHappened(3, Times.Exactly);
    }
    
    [Fact]
    public async Task Handle_WhenCalled_ShouldRemoveTicketAsync()
    {
        // Act
        await sut.Handle(e, cancellationToken);

        // Assert
        A.CallTo(() => ticketRepository.RemoveAsync(ticket, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }
}