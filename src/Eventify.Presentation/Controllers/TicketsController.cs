using Eventify.Application.Tickets.Commands.CreateTicket;
using Eventify.Application.Tickets.Commands.RemoveTicket;
using Eventify.Application.Tickets.Commands.UpdateTicket;
using Eventify.Application.Tickets.Queries.GetTicket;
using Eventify.Contracts.Tickets.Requests;

namespace Eventify.Presentation.Controllers;

public sealed class TicketsController : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateTicket(CreateTicketRequest request,
        CancellationToken cancellationToken)
    {
        var result = await SendAsync(request.Adapt<CreateTicketCommand>(), cancellationToken);

        return result.Match(
            onValue: response => CreatedAtAction(
                actionName: nameof(GetTicket),
                routeValues: new { response.Id },
                value: response),
            onError: Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTicket(Guid id, CancellationToken cancellationToken)
    {
        var result = await SendAsync(new GetTicketQuery
        {
            TicketId = id
        }, cancellationToken);

        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpPost("{id:guid}")]
    public async Task<IActionResult> UpdateTicket(Guid id, UpdateTicketRequest request,
        CancellationToken cancellationToken)
    {
        var result = await SendAsync(request.Adapt<UpdateTicketCommand>() with
        {
            TicketId = id
        }, cancellationToken);

        return result.Match(
            onValue: _ => NoContent(),
            onError: Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> RemoveTicket(Guid id, CancellationToken cancellationToken)
    {
        var result = await SendAsync(new RemoveTicketCommand
        {
            TicketId = id
        }, cancellationToken);

        return result.Match(
            onValue: _ => NoContent(),
            onError: Problem);
    }
}