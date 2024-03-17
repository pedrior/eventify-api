using Eventify.Domain.Attendees.ValueObjects;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Producers.ValueObjects;

namespace Eventify.Application.Common.Abstractions.Storage;

internal static class StorageKeys
{
    public static Uri AttendeePicture(AttendeeId attendeeId) =>
        new($"attendees/{attendeeId}/picture", UriKind.Relative);

    public static Uri ProducerPicture(ProducerId producerId) =>
        new($"producers/{producerId}/picture", UriKind.Relative);

    public static Uri EventPoster(EventId eventId) =>
        new($"events/{eventId}/poster", UriKind.Relative);
}