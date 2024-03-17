using Eventify.Application.Events.Events;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Events.Events;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.UnitTests.Events.Events;

[TestSubject(typeof(EventUnpublishedHandler))]
public sealed class EventUnpublishedHandlerTests
{
    private readonly IBookingRepository bookingRepository = A.Fake<IBookingRepository>();
    private readonly ILogger<EventUnpublishedHandler> logger = A.Fake<ILogger<EventUnpublishedHandler>>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly EventUnpublished e = new(Constants.Event.EventId);

    private readonly EventUnpublishedHandler sut;

    public EventUnpublishedHandlerTests()
    {
        sut = new EventUnpublishedHandler(bookingRepository, logger);
    }
    
    [Fact]
    public async Task Handle_WhenCalled_ShouldCancelAllEventBookingsAsync()
    {
        // Arrange
        A.CallTo(() => bookingRepository.ListByEventAsync(e.EventId, cancellationToken))
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
}