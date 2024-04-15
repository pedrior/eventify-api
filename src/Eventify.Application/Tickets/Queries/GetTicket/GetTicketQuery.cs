using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Contracts.Tickets.Responses;

namespace Eventify.Application.Tickets.Queries.GetTicket;

public sealed record GetTicketQuery : IQuery<TicketResponse>
{
    public required Guid TicketId { get; init; }
}