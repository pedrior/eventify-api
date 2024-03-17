using Eventify.Domain.Common.Events;

namespace Eventify.Domain.Tickets.Events;

public sealed record TicketCreated(Ticket Ticket) : IDomainEvent;