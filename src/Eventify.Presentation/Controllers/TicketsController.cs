using Eventify.Application.Tickets.Commands.CreateTicket;
using Eventify.Application.Tickets.Commands.RemoveTicket;
using Eventify.Application.Tickets.Commands.UpdateTicket;
using Eventify.Application.Tickets.Queries.GetTicket;
using Eventify.Contracts.Tickets.Requests;

namespace Eventify.Presentation.Controllers;

public sealed class TicketsController : ApiController
{
    [HttpPost]
    public Task<IActionResult> CreateTicket(CreateTicketRequest request,
        CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<CreateTicketCommand>(), cancellationToken)
            .ToResponseAsync(response => CreatedAtAction(
                    nameof(GetTicket),
                    new { id = response.Id },
                    response),
                HttpContext);
    }

    [HttpGet("{id:guid}")]
    public Task<IActionResult> GetTicket(Guid id, CancellationToken cancellationToken)
    {
        return SendAsync(new GetTicketQuery
            {
                TicketId = id
            }, cancellationToken)
            .ToResponseAsync(Ok, HttpContext);
    }

    [HttpPost("{id:guid}")]
    public Task<IActionResult> UpdateTicket(Guid id, UpdateTicketRequest request,
        CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<UpdateTicketCommand>() with
            {
                TicketId = id
            }, cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }

    [HttpDelete("{id:guid}")]
    public Task<IActionResult> RemoveTicket(Guid id, CancellationToken cancellationToken)
    {
        return SendAsync(new RemoveTicketCommand
            {
                TicketId = id
            }, cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }
}