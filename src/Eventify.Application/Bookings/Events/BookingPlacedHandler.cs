using Eventify.Application.Common.Abstractions.Requests;
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

        await attendee.AddBooking(booking)
            .ThrowIfFailure()
            .ThenAsync(() => attendeeRepository.UpdateAsync(attendee, cancellationToken));
    }

    private async Task ProcessBookingPaymentAsync(Booking booking, CancellationToken cancellationToken)
    {
        await (Random.Shared.Next(10) switch
            {
                > 4 => booking.Pay(),
                _ => booking.Cancel(CancellationReason.PaymentFailed)
            })
            .ThrowIfFailure()
            .ThenAsync(_ => bookingRepository.UpdateAsync(booking, cancellationToken));
    }
}