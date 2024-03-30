using Eventify.Domain.Attendees.Errors;
using Eventify.Domain.Attendees.Events;
using Eventify.Domain.Attendees.ValueObjects;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Common.Entities;
using Eventify.Domain.Users.ValueObjects;

namespace Eventify.Domain.Attendees;

public sealed class Attendee : Entity<AttendeeId>, IAggregateRoot, IAuditable
{
    private readonly List<BookingId> bookingIds = [];

    private Attendee(AttendeeId id) : base(id)
    {
    }

    public required UserId UserId { get; init; }

    public AttendeeDetails Details { get; private set; } = default!;

    public AttendeeContact Contact { get; private set; } = default!;

    public Uri? PictureUrl { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public IReadOnlyCollection<BookingId> BookingIds => bookingIds.AsReadOnly();

    public static Attendee Create(
        AttendeeId attendeeId,
        UserId userId,
        AttendeeDetails details,
        AttendeeContact contact,
        Uri? pictureUrl = null)
    {
        var attendee = new Attendee(attendeeId)
        {
            UserId = userId,
            Details = details,
            Contact = contact,
            PictureUrl = pictureUrl
        };

        attendee.RaiseDomainEvent(new AttendeeCreated(attendee));

        return attendee;
    }

    public Result<Success> UpdateProfile(AttendeeDetails details, AttendeeContact contact)
    {
        Details = details;
        Contact = contact;

        return Success.Value;
    }

    public Result<Success> SetPicture(Uri pictureUrl)
    {
        PictureUrl = pictureUrl;
        return Success.Value;
    }

    public Result<Success> RemovePicture()
    {
        PictureUrl = null;
        return Success.Value;
    }

    public Result<Success> AddBooking(Booking booking)
    {
        if (bookingIds.Contains(booking.Id))
        {
            return AttendeeErrors.BookingAlreadyAdded(booking.Id);
        }

        bookingIds.Add(booking.Id);

        return Success.Value;
    }

    public Result<Success> RequestBookingCancellation(Booking booking)
    {
        if (!bookingIds.Contains(booking.Id))
        {
            return AttendeeErrors.BookingNotFound(booking.Id);
        }

        if (booking.IsCancelled)
        {
            return AttendeeErrors.BookingAlreadyCancelled(booking.Id);
        }

        RaiseDomainEvent(new BookingCancellationRequested(booking.Id));

        return Success.Value;
    }
}