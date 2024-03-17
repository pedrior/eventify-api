using Eventify.Application.Bookings.Common.Errors;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Users;

namespace Eventify.Application.Bookings.Commands.CancelBooking;

internal sealed class CancelBookingCommandHandler(
    IUser user,
    IAttendeeRepository attendeeRepository,
    IBookingRepository bookingRepository
) : ICommandHandler<CancelBookingCommand, Success>
{
    public async Task<ErrorOr<Success>> Handle(CancelBookingCommand command,
        CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetAsync(new BookingId(command.BookingId), cancellationToken);
        if (booking is null)
        {
            return BookingErrors.NotFound(command.BookingId);
        }

        var attendee = await attendeeRepository.GetByUserAsync(user.Id, cancellationToken);
        if (attendee is null)
        {
            throw new ApplicationException("Attendee not found");
        }

        return await attendee.RequestBookingCancellation(booking)
            .ThenAsync(_ => attendeeRepository.UpdateAsync(attendee, cancellationToken));
    }
}