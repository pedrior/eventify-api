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
    public async Task<IActionResult> CreateEvent(CreateEventRequest request, CancellationToken cancellationToken)
    {
        var result = await SendAsync(request.Adapt<CreateEventCommand>(), cancellationToken);
        return result.Match(
            onValue: response => CreatedAtAction(
                actionName: nameof(GetEventEditable),
                routeValues: new { response.Id },
                value: response),
            onError: Problem);
    }

    [HttpPost("{id:guid}/publish")]
    public async Task<IActionResult> Publish(Guid id, CancellationToken cancellationToken)
    {
        var result = await SendAsync(new PublishEventCommand { EventId = id }, cancellationToken);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpPost("{id:guid}/unpublish")]
    public async Task<IActionResult> Unpublish(Guid id, CancellationToken cancellationToken)
    {
        var result = await SendAsync(new UnpublishEventCommand { EventId = id }, cancellationToken);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpPost("{id:guid}/poster")]
    [RequestSizeLimit(RequestConstants.ImageSizeLimit)]
    public async Task<IActionResult> UploadPoster(Guid id, [FromForm(Name = "poster")] IFormFile poster,
        CancellationToken cancellationToken)
    {
        var result = await SendAsync(new UploadPosterCommand
        {
            EventId = id,
            Poster = new FileProxy(poster)
        }, cancellationToken);

        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpGet("{idOrSlug}"), AllowAnonymous]
    public async Task<IActionResult> GetEvent(string idOrSlug, CancellationToken cancellationToken)
    {
        var result = await SendAsync(new GetEventQuery { IdOrSlug = idOrSlug }, cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpGet("{id:guid}/edit")]
    public async Task<IActionResult> GetEventEditable(Guid id, CancellationToken cancellationToken)
    {
        var result = await SendAsync(new GetEventEditableQuery { EventId = id }, cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpGet, AllowAnonymous]
    public async Task<IActionResult> GetEvents([FromQuery] GetEventsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await SendAsync(request.Adapt<GetEventsQuery>(), cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpPut("{id:guid}/details")]
    public async Task<IActionResult> UpdateDetails(Guid id, UpdateEventDetailsRequest request,
        CancellationToken cancellationToken)
    {
        var result = await SendAsync(request.Adapt<UpdateEventDetailsCommand>() with
        {
            EventId = id
        }, cancellationToken);

        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpPut("{id:guid}/location")]
    public async Task<IActionResult> UpdateLocation(Guid id, UpdateEventLocationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await SendAsync(request.Adapt<UpdateEventLocationCommand>() with
        {
            EventId = id
        }, cancellationToken);

        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpPut("{id:guid}/period")]
    public async Task<IActionResult> UpdatePeriod(Guid id, UpdateEventPeriodRequest request,
        CancellationToken cancellationToken)
    {
        var result = await SendAsync(request.Adapt<UpdateEventPeriodCommand>() with
        {
            EventId = id
        }, cancellationToken);

        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpPut("{id:guid}/slug")]
    public async Task<IActionResult> UpdateSlug(Guid id, UpdateEventSlugRequest request,
        CancellationToken cancellationToken)
    {
        var result = await SendAsync(request.Adapt<UpdateEventSlugCommand>() with
        {
            EventId = id
        }, cancellationToken);

        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await SendAsync(new DeleteEventCommand { EventId = id }, cancellationToken);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpDelete("{id:guid}/poster")]
    public async Task<IActionResult> RemovePoster(Guid id, CancellationToken cancellationToken)
    {
        var result = await SendAsync(new RemoveEventPosterCommand { EventId = id }, cancellationToken);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }
}