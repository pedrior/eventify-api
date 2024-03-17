using Eventify.Domain.Common.Entities;
using Eventify.Domain.Events;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Producers.Errors;
using Eventify.Domain.Producers.Events;
using Eventify.Domain.Producers.ValueObjects;
using Eventify.Domain.Users.ValueObjects;

namespace Eventify.Domain.Producers;

public sealed class Producer : Entity<ProducerId>, IAggregateRoot, IAuditable
{
    private readonly List<EventId> eventIds = [];
    private readonly List<EventId> deletedEventIds = [];

    private Producer(ProducerId id) : base(id)
    {
    }

    public required UserId UserId { get; init; }

    public ProducerDetails Details { get; private set; } = default!;

    public ProducerContact Contact { get; private set; } = default!;

    public Uri? PictureUrl { get; private set; }

    public Uri? WebsiteUrl { get; private set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public IReadOnlyCollection<EventId> EventIds => eventIds.AsReadOnly();

    public IReadOnlyCollection<EventId> DeletedEventIds => deletedEventIds.AsReadOnly();

    public static Producer Create(
        ProducerId producerId,
        UserId userId,
        ProducerDetails details,
        ProducerContact contact,
        Uri? pictureUrl = null,
        Uri? websiteUrl = null)
    {
        var producer = new Producer(producerId)
        {
            UserId = userId,
            Details = details,
            Contact = contact,
            PictureUrl = pictureUrl,
            WebsiteUrl = websiteUrl
        };

        producer.RaiseDomainEvent(new ProducerCreated(producer));

        return producer;
    }

    public ErrorOr<Updated> UpdateProfile(ProducerDetails details, ProducerContact contact, Uri? websiteUrl = null)
    {
        Details = details;
        Contact = contact;
        WebsiteUrl = websiteUrl;

        return Result.Updated;
    }

    public ErrorOr<Success> SetPicture(Uri pictureUrl)
    {
        PictureUrl = pictureUrl;
        return Result.Success;
    }

    public ErrorOr<Deleted> RemovePicture()
    {
        PictureUrl = null;
        return Result.Deleted;
    }

    public ErrorOr<Success> AddEvent(Event @event)
    {
        if (eventIds.Contains(@event.Id))
        {
            return ProducerErrors.EventAlreadyAdded(@event.Id);
        }

        eventIds.Add(@event.Id);

        return Result.Success;
    }

    public ErrorOr<Deleted> DeleteEvent(Event @event)
    {
        if (!eventIds.Contains(@event.Id))
        {
            return ProducerErrors.EventNotFound(@event.Id);
        }

        if (@event.IsOngoing)
        {
            return ProducerErrors.CannotDeleteOngoingEvent(@event.Id);
        }

        eventIds.Remove(@event.Id);
        deletedEventIds.Add(@event.Id);

        RaiseDomainEvent(new EventDeleted(@event));

        return Result.Deleted;
    }

    public ErrorOr<Success> PublishEvent(Event @event) => !eventIds.Contains(@event.Id)
        ? ProducerErrors.EventNotFound(@event.Id)
        : @event.Publish();

    public ErrorOr<Success> UnpublishEvent(Event @event) => !eventIds.Contains(@event.Id)
        ? ProducerErrors.EventNotFound(@event.Id)
        : @event.Unpublish();
}