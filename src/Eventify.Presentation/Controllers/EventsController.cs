using Eventify.Application.Events.Commands.CreateEvent;
using Eventify.Application.Events.Commands.DeleteEvent;
using Eventify.Application.Events.Commands.PublishEvent;
using Eventify.Application.Events.Commands.RemovePoster;
using Eventify.Application.Events.Commands.UnpublishEvent;
using Eventify.Application.Events.Commands.UpdateDetails;
using Eventify.Application.Events.Commands.UpdateLocation;
using Eventify.Application.Events.Commands.UpdatePeriod;
using Eventify.Application.Events.Commands.UpdateSlug;
using Eventify.Application.Events.Commands.UploadPoster;
using Eventify.Application.Events.Queries.GetEvent;
using Eventify.Application.Events.Queries.GetEventEditable;
using Eventify.Application.Events.Queries.GetEvents;
using Eventify.Contracts.Events.Requests;

namespace Eventify.Presentation.Controllers;

[ApiVersion(1)]
public sealed class EventsController : ApiController
{
    [HttpPost]
    public Task<IActionResult> CreateEvent(CreateEventRequest request, CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<CreateEventCommand>(), cancellationToken)
            .ToResponseAsync(response => CreatedAtAction(
                    nameof(GetEvent),
                    new { idOrSlug = response.Id },
                    response),
                HttpContext);
    }

    [HttpPost("{id:guid}/publish")]
    public Task<IActionResult> Publish(Guid id, CancellationToken cancellationToken)
    {
        return SendAsync(new PublishEventCommand { EventId = id }, cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }

    [HttpPost("{id:guid}/unpublish")]
    public Task<IActionResult> Unpublish(Guid id, CancellationToken cancellationToken)
    {
        return SendAsync(new UnpublishEventCommand { EventId = id }, cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }

    [HttpPost("{id:guid}/poster")]
    [RequestSizeLimit(RequestConstants.ImageSizeLimit)]
    public Task<IActionResult> UploadPoster(Guid id, [FromForm(Name = "poster")] IFormFile poster,
        CancellationToken cancellationToken)
    {
        return SendAsync(new UploadPosterCommand
            {
                EventId = id,
                Poster = new FileProxy(poster)
            }, cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }

    [HttpGet("{idOrSlug}"), AllowAnonymous]
    public Task<IActionResult> GetEvent(string idOrSlug, CancellationToken cancellationToken)
    {
        return SendAsync(new GetEventQuery { IdOrSlug = idOrSlug }, cancellationToken)
            .ToResponseAsync(Ok, HttpContext);
    }

    [HttpGet("{id:guid}/edit")]
    public Task<IActionResult> GetEventEditable(Guid id, CancellationToken cancellationToken)
    {
        return SendAsync(new GetEventEditableQuery { EventId = id }, cancellationToken)
            .ToResponseAsync(Ok, HttpContext);
    }

    [HttpGet, AllowAnonymous]
    public Task<IActionResult> GetEvents([FromQuery] GetEventsRequest request,
        CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<GetEventsQuery>(), cancellationToken)
            .ToResponseAsync(Ok, HttpContext);
    }

    [HttpPost("{id:guid}/details")]
    public Task<IActionResult> UpdateDetails(Guid id, UpdateEventDetailsRequest request,
        CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<UpdateEventDetailsCommand>() with
            {
                EventId = id
            }, cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }

    [HttpPost("{id:guid}/location")]
    public Task<IActionResult> UpdateLocation(Guid id, UpdateEventLocationRequest request,
        CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<UpdateEventLocationCommand>() with
            {
                EventId = id
            }, cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }

    [HttpPost("{id:guid}/period")]
    public Task<IActionResult> UpdatePeriod(Guid id, UpdateEventPeriodRequest request,
        CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<UpdateEventPeriodCommand>() with
            {
                EventId = id
            }, cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }

    [HttpPost("{id:guid}/slug")]
    public Task<IActionResult> UpdateSlug(Guid id, UpdateEventSlugRequest request,
        CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<UpdateEventSlugCommand>() with
            {
                EventId = id
            }, cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }

    [HttpDelete("{id:guid}")]
    public Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        return SendAsync(new DeleteEventCommand { EventId = id }, cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }

    [HttpDelete("{id:guid}/poster")]
    public Task<IActionResult> RemovePoster(Guid id, CancellationToken cancellationToken)
    {
        return SendAsync(new RemoveEventPosterCommand { EventId = id }, cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }
}