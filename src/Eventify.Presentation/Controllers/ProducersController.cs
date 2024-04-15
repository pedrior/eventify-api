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
    [HttpPost("profile")]
    public Task<IActionResult> CreateProfile(CreateProducerProfileRequest request,
        CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<CreateProducerProfileCommand>(), cancellationToken)
            .ToResponseAsync(response => CreatedAtAction(
                    nameof(GetProfile),
                    new { },
                    response),
                HttpContext);
    }

    [HttpPost("profile-picture")]
    [RequestSizeLimit(RequestConstants.ImageSizeLimit)]
    public Task<IActionResult> UpdatePicture([FromForm(Name = "picture")] IFormFile picture,
        CancellationToken cancellationToken)
    {
        return SendAsync(new UploadProducerPictureCommand
            {
                Picture = new FileProxy(picture)
            }, cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }

    [HttpGet("profile")]
    public Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        return SendAsync(new GetProducerProfileQuery(), cancellationToken)
            .ToResponseAsync(Ok, HttpContext);
    }

    [HttpGet("events")]
    public Task<IActionResult> GetEvents(CancellationToken cancellationToken)
    {
        return SendAsync(new GetProducerEventsQuery(), cancellationToken)
            .ToResponseAsync(Ok, HttpContext);
    }

    [HttpPost("update-profile")]
    public Task<IActionResult> UpdateProfile(UpdateProducerProfileRequest request,
        CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<UpdateProducerProfileCommand>(), cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }

    [HttpDelete("profile-picture")]
    public Task<IActionResult> RemovePicture(CancellationToken cancellationToken)
    {
        return SendAsync(new RemoveProducerPictureCommand(), cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }
}