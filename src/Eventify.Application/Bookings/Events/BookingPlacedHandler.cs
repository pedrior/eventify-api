using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Enums;
using Eventify.Domain.Bookings.Events;
using Eventify.Domain.Bookings.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Bookings.Events;

internal sealed class BookingPlacedHandler(
    IAttendeeRepository attendeeRepository,
    IBookingRepository bookingRepository,
    ILogger<BookingPlacedHandler> logger
) : IDomainEventHandler<BookingPlaced>
{
    public async Task Handle(BookingPlaced e, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Event} domain event", nameof(BookingPlaced));

        await AddBookingToAttendeeAsync(e.Booking, cancellationToken);
        await ProcessBookingPaymentAsync(e.Booking, cancellationToken);
    }
    
    private async Task AddBookingToAttendeeAsync(Booking booking, CancellationToken cancellationToken)
    {
        var attendee = await attendeeRepository.GetAsync(booking.AttendeeId, cancellationToken);
        if (attendee is null)
        {
            throw new ApplicationException($"Attendee {booking.AttendeeId} not found");
        }

        var result = await attendee.AddBooking(booking)
            .ThenAsync(_ => attendeeRepository.UpdateAsync(attendee, cancellationToken));

        if (result.IsError)
        {
            throw new ApplicationException($"Failed to add booking {booking.Id} to " +
                                           $"attendee {booking.AttendeeId}: {result.FirstError.Description}");
        }
    }

    private async Task ProcessBookingPaymentAsync(Booking booking, CancellationToken cancellationToken)
    {
        var result = await (Random.Shared.Next(10) switch
        {
            > 4 => booking.Pay(),
            _ => booking.Cancel(CancellationReason.PaymentFailed)
        }).ThenAsync(_ => bookingRepository.UpdateAsync(booking, cancellationToken));

        if (result.IsError)
        {
            throw new ApplicationException(
                $"Failed to process payment for booking {booking.Id}: {result.FirstError.Description}");
        }
    }
}