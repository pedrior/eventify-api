using Eventify.Application.Attendees.Commands.RemovePicture;
using Eventify.Application.Attendees.Commands.UpdateProfile;
using Eventify.Application.Attendees.Commands.UploadPicture;
using Eventify.Application.Attendees.Queries.GetBookings;
using Eventify.Application.Attendees.Queries.GetProfile;
using Eventify.Contracts.Attendees.Requests;

namespace Eventify.Presentation.Controllers;

[ApiVersion(1)]
public sealed class AttendeesController : ApiController
{
    [HttpPost("picture")]
    [RequestSizeLimit(RequestConstants.ImageSizeLimit)]
    public async Task<IActionResult> UploadPicture([FromForm(Name = "picture")] IFormFile picture,
        CancellationToken cancellationToken)
    {
        var result = await SendAsync(new UploadAttendeePictureCommand
        {
            Picture = new FileProxy(picture)
        }, cancellationToken);

        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpGet]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var result = await SendAsync(new GetAttendeeProfileQuery(), cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }
    
    [HttpGet("bookings")]
    public async Task<IActionResult> GetBookings(CancellationToken cancellationToken)
    {
        var result = await SendAsync(new GetAttendeeBookingsQuery(), cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile(UpdateAttendeeProfileRequest request,
        CancellationToken cancellationToken)
    {
        var result = await SendAsync(request.Adapt<UpdateAttendeeProfileCommand>(), cancellationToken);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpDelete("picture")]
    public async Task<IActionResult> RemovePicture(CancellationToken cancellationToken)
    {
        var result = await SendAsync(new RemoveAttendeePictureCommand(), cancellationToken);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }
}