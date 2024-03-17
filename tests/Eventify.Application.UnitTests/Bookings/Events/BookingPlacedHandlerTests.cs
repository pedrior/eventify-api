using Eventify.Application.Bookings.Events;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Events;
using Eventify.Domain.Bookings.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.UnitTests.Bookings.Events;

[TestSubject(typeof(BookingPlacedHandler))]
public sealed class BookingPlacedHandlerTests
{
    private readonly IBookingRepository bookingRepository = A.Fake<IBookingRepository>();
    private readonly ILogger<BookingPlacedHandler> logger = A.Fake<ILogger<BookingPlacedHandler>>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly BookingPlacedHandler sut;

    private readonly Booking booking = Factories.Booking.CreateBooking();

    private readonly BookingPlaced e;

    public BookingPlacedHandlerTests()
    {
        sut = new BookingPlacedHandler(bookingRepository, logger);

        e = new BookingPlaced(booking);

        A.CallTo(() => bookingRepository.GetAsync(e.Booking.Id, cancellationToken))
            .Returns(booking);
    }
    
    [Fact]
    public async Task Handle_WhenProcessed_ShouldUpdatedBooking()
    {
        // Arrange
        // Act
        await sut.Handle(e, cancellationToken);

        // Assert
        A.CallTo(() => bookingRepository.UpdateAsync(booking, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }
}