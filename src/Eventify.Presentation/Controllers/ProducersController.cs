using Eventify.Application.Producers.Commands.CreateProfile;
using Eventify.Application.Producers.Commands.RemovePicture;
using Eventify.Application.Producers.Commands.UpdateProfile;
using Eventify.Application.Producers.Commands.UploadPicture;
using Eventify.Application.Producers.Queries.GetEvents;
using Eventify.Application.Producers.Queries.GetProfile;
using Eventify.Contracts.Producers.Requests;

namespace Eventify.Presentation.Controllers;

[ApiVersion(1)]
public sealed class ProducersController : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateProfile(CreateProducerProfileRequest request,
        CancellationToken cancellationToken)
    {
        var result = await SendAsync(request.Adapt<CreateProducerProfileCommand>(), cancellationToken);
        return result.Match(onValue: _ => CreatedAtAction(nameof(GetProfile), value: null), onError: Problem);
    }

    [HttpPost("picture")]
    [RequestSizeLimit(RequestConstants.ImageSizeLimit)]
    public async Task<IActionResult> UpdatePicture([FromForm(Name = "picture")] IFormFile picture,
        CancellationToken cancellationToken)
    {
        var result = await SendAsync(new UploadProducerPictureCommand
        {
            Picture = new FileProxy(picture)
        }, cancellationToken);

        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var result = await SendAsync(new GetProducerProfileQuery(), cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpGet("events")]
    public async Task<IActionResult> GetEvents(CancellationToken cancellationToken)
    {
        var result = await SendAsync(new GetProducerEventsQuery(), cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProfile(UpdateProducerProfileRequest request,
        CancellationToken cancellationToken)
    {
        var result = await SendAsync(request.Adapt<UpdateProducerProfileCommand>(), cancellationToken);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpDelete("picture")]
    public async Task<IActionResult> RemovePicture(CancellationToken cancellationToken)
    {
        var result = await SendAsync(new RemoveProducerPictureCommand(), cancellationToken);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }
}