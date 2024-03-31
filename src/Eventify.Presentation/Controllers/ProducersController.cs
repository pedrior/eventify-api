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
    public Task<IActionResult> CreateProfile(CreateProducerProfileRequest request,
        CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<CreateProducerProfileCommand>(), cancellationToken)
            .CreatedAtAction(
                actionName: nameof(GetProfile),
                routeValues: null,
                context: HttpContext);
    }

    [HttpPost("picture")]
    [RequestSizeLimit(RequestConstants.ImageSizeLimit)]
    public Task<IActionResult> UpdatePicture([FromForm(Name = "picture")] IFormFile picture,
        CancellationToken cancellationToken)
    {
        return SendAsync(new UploadProducerPictureCommand
            {
                Picture = new FileProxy(picture)
            }, cancellationToken)
            .NoContent(HttpContext);
    }

    [HttpGet]
    public Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        return SendAsync(new GetProducerProfileQuery(), cancellationToken)
            .Ok(HttpContext);
    }

    [HttpGet("events")]
    public Task<IActionResult> GetEvents(CancellationToken cancellationToken)
    {
        return SendAsync(new GetProducerEventsQuery(), cancellationToken)
            .Ok(HttpContext);
    }

    [HttpPost]
    public Task<IActionResult> UpdateProfile(UpdateProducerProfileRequest request,
        CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<UpdateProducerProfileCommand>(), cancellationToken)
            .NoContent(HttpContext);
    }

    [HttpDelete("picture")]
    public Task<IActionResult> RemovePicture(CancellationToken cancellationToken)
    {
        return SendAsync(new RemoveProducerPictureCommand(), cancellationToken)
            .NoContent(HttpContext);
    }
}