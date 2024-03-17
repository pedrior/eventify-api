using Eventify.Application.Common.Abstractions.Security;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Application.Common.Security.Requirements;

internal sealed record TicketOwnerRequirement(TicketId TicketId) : IRequirement;