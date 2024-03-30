using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Common.Entities;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.Enums;
using Eventify.Domain.Events.Errors;
using Eventify.Domain.Events.Events;
using Eventify.Domain.Events.Services;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Producers.ValueObjects;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Domain.Events;

public sealed class Event : Entity<EventId>, IAggregateRoot, IAuditable, ISoftDelete
{
    public const int TicketsLimit = 10;

    private readonly List<TicketId> ticketIds = [];
    private readonly List<BookingId> bookingIds = [];

    private Event(EventId id) : base(id)
    {
    }

    public required ProducerId ProducerId { get; init; }

    public Slug Slug { get; private set; } = default!;

    public EventDetails Details { get; private set; } = default!;

    public EventLocation Location { get; private set; } = default!;

    public Period Period { get; private set; } = default!;

    public EventState State { get; private set; } = default!;

    public Uri? PosterUrl { get; private set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }

    public DateTimeOffset? PublishedAt { get; private set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public bool IsPublished => State == EventState.Published;

    public bool IsFinished => State == EventState.Published && Period.IsPast;
    
    public bool IsOngoing => State == EventState.Published && !Period.IsPast;

    public bool HasTickets => ticketIds.Count is not 0;

    public IReadOnlyCollection<TicketId> TicketIds => ticketIds.AsReadOnly();

    public IReadOnlyCollection<BookingId> BookingIds => bookingIds.AsReadOnly();

    public static Event Create(
        EventId eventId,
        ProducerId producerId,
        EventDetails details,
        EventLocation location,
        Period period)
    {
        var @event = new Event(eventId)
        {
            ProducerId = producerId,

            Details = details,
            Location = location,
            Period = period,
            State = EventState.Draft,
            Slug = Slug.Create(details.Name, randomSuffix: true)
        };

        @event.RaiseDomainEvent(new EventCreated(@event));

        return @event;
    }

    public Result<Success> UpdateDetails(EventDetails details)
    {
        if (IsFinished)
        {
            return EventErrors.CannotModifyFinishedEvent;
        }
        
        Details = details;
        return Success.Value;
    }

    public Result<Success> UpdatePeriod(Period period)
    {
        if (IsFinished)
        {
            return EventErrors.CannotModifyFinishedEvent;
        }
        
        if (IsPublished)
        {
            return EventErrors.InvalidOperation(State);
        }

        Period = period;
        return Success.Value;
    }

    public Result<Success> UpdateLocation(EventLocation location)
    {
        if (IsFinished)
        {
            return EventErrors.CannotModifyFinishedEvent;
        }
        
        if (IsPublished)
        {
            return EventErrors.InvalidOperation(State);
        }

        Location = location;
        return Success.Value;
    }

    public async Task<Result<Success>> UpdateSlugAsync(
        Slug slug,
        IEventSlugUniquenessChecker uniquenessChecker,
        CancellationToken cancellationToken = default)
    {
        if (IsFinished)
        {
            return EventErrors.CannotModifyFinishedEvent;
        }
        
        if (!await uniquenessChecker.IsUniqueAsync(slug, cancellationToken))
        {
            return EventErrors.SlugAlreadyExists(slug);
        }

        Slug = slug;
        return Success.Value;
    }

    public Result<Success> SetPoster(Uri posterUrl)
    {
        if (IsFinished)
        {
            return EventErrors.CannotModifyFinishedEvent;
        }
        
        PosterUrl = posterUrl;
        return Success.Value;
    }

    public Result<Success> RemovePoster()
    {
        if (IsFinished)
        {
            return EventErrors.CannotModifyFinishedEvent;
        }
        
        PosterUrl = null;
        return Success.Value;
    }

    public Result<Success> Publish()
    {
        if (!State.CanTransitionTo(EventState.Published))
        {
            return EventErrors.InvalidOperation(State);
        }

        if (Period.IsPast)
        {
            return EventErrors.MustBeInFuture;
        }

        if (!HasTickets)
        {
            return EventErrors.MustHaveTicket;
        }

        State = EventState.Published;
        PublishedAt = DateTimeOffset.UtcNow;

        RaiseDomainEvent(new EventPublished(this));
        return Success.Value;
    }

    public Result<Success> Unpublish()
    {
        if (!State.CanTransitionTo(EventState.Draft))
        {
            return EventErrors.InvalidOperation(State);
        }

        State = EventState.Draft;
        PublishedAt = null;

        RaiseDomainEvent(new EventUnpublished(Id));
        return Success.Value;
    }

    public Result<Success> AddTicket(Ticket ticket)
    {
        if (IsFinished)
        {
            return EventErrors.CannotModifyFinishedEvent;
        }

        if (ticketIds.Contains(ticket.Id))
        {
            return EventErrors.TicketAlreadyAdded(ticket.Id);
        }

        if (ticket.SaleEnd is not null && ticket.SaleEnd > Period.End)
        {
            return EventErrors.TicketSalePeriodExceedsEventPeriod;
        }

        if (ticketIds.Count is TicketsLimit)
        {
            return EventErrors.TicketLimitReached(TicketsLimit);
        }

        ticketIds.Add(ticket.Id);
        return Success.Value;
    }

    public Result<Success> RemoveTicket(Ticket ticket)
    {
        if (IsFinished)
        {
            return EventErrors.CannotModifyFinishedEvent;
        }
        
        if (!ticketIds.Contains(ticket.Id))
        {
            return EventErrors.TicketNotFound(ticket.Id);
        }

        ticketIds.Remove(ticket.Id);
        RaiseDomainEvent(new TicketRemoved(ticket));
        return Success.Value;
    }

    public Result<Success> AddBooking(Booking booking)
    {
        if (IsFinished)
        {
            return EventErrors.CannotModifyFinishedEvent;
        }
        
        if (!IsPublished)
        {
            return EventErrors.InvalidOperation(State);
        }

        if (bookingIds.Contains(booking.Id))
        {
            return EventErrors.BookingAlreadyAdded(booking.Id);
        }

        if (!ticketIds.Contains(booking.TicketId))
        {
            return EventErrors.TicketNotFound(booking.TicketId);
        }

        bookingIds.Add(booking.Id);
        return Success.Value;
    }
}