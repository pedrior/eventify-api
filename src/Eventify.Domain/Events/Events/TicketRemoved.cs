using Eventify.Domain.Common.Events;
using Eventify.Domain.Tickets;

namespace Eventify.Domain.Events.Events;

public sealed record TicketRemoved(Ticket Ticket) : IDomainEvent;