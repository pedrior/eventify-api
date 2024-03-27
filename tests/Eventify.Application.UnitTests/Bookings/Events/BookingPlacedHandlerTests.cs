using Eventify.Application.Bookings.Events;
using Eventify.Domain.Attendees;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Events;
using Eventify.Domain.Bookings.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.UnitTests.Bookings.Events;

[TestSubject(typeof(BookingPlacedHandler))]
public sealed class BookingPlacedHandlerTests
{
    private readonly IAttendeeRepository attendeeRepository = A.Fake<IAttendeeRepository>();
    private readonly IBookingRepository bookingRepository = A.Fake<IBookingRepository>();
    private readonly ILogger<BookingPlacedHandler> logger = A.Fake<ILogger<BookingPlacedHandler>>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly BookingPlacedHandler sut;

    private readonly Attendee attendee = Factories.Attendee.CreateAttendee();
    private readonly Booking booking = Factories.Booking.CreateBooking();

    private readonly BookingPlaced e;

    public BookingPlacedHandlerTests()
    {
        sut = new BookingPlacedHandler(attendeeRepository, bookingRepository, logger);

        e = new BookingPlaced(booking);
        
        A.CallTo(() => attendeeRepository.GetAsync(booking.AttendeeId, cancellationToken))
            .Returns(attendee);

        A.CallTo(() => bookingRepository.GetAsync(e.Booking.Id, cancellationToken))
            .Returns(booking);
    }
    
    [Fact]
    public async Task Handle_WhenCalled_ShouldAddBookingToAttendee()
    {
        // Arrange
        // Act
        await sut.Handle(e, cancellationToken);

        // Assert
        A.CallTo(() => attendeeRepository.UpdateAsync(
                A<Attendee>.That.Matches(a => a.BookingIds.Contains(booking.Id)), cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenAttendeeDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => attendeeRepository.GetAsync(booking.AttendeeId, cancellationToken))
            .Returns(null as Attendee);

        // Act
        var act = () => sut.Handle(e, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage($"Attendee {booking.AttendeeId} not found");
    }

    [Fact]
    public async Task Handle_WhenAddBookingToAttendeeFails_ShouldThrowApplicationException()
    {
        // Arrange
        attendee.AddBooking(booking);

        // Act
        var act = () => sut.Handle(e, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage($"Failed to add booking {booking.Id} to attendee {booking.AttendeeId}: *");
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