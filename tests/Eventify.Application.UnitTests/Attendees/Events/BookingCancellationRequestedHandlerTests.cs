using Eventify.Application.Attendees.Events;
using Eventify.Domain.Attendees.Events;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.UnitTests.Attendees.Events;

[TestSubject(typeof(BookingCancellationRequestedHandler))]
public sealed class BookingCancellationRequestedHandlerTests
{
    private readonly IBookingRepository bookingRepository = A.Fake<IBookingRepository>();

    private readonly ILogger<BookingCancellationRequestedHandler> logger =
        A.Fake<ILogger<BookingCancellationRequestedHandler>>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly Booking booking = Factories.Booking.CreateBooking();

    private readonly BookingCancellationRequested e;
    private readonly BookingCancellationRequestedHandler sut;

    public BookingCancellationRequestedHandlerTests()
    {
        e = new BookingCancellationRequested(booking.Id);
        sut = new BookingCancellationRequestedHandler(bookingRepository, logger);

        A.CallTo(() => bookingRepository.GetAsync(booking.Id, cancellationToken))
            .Returns(booking);
    }

    [Fact]
    public async Task Handle_WhenBookingDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => bookingRepository.GetAsync(booking.Id, cancellationToken))
            .Returns(null as Booking);

        // Acts
        var act = () => sut.Handle(e, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage($"Booking {booking.Id} not found");
    }
}