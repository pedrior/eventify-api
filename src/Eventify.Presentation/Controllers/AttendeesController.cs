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
    public Task<IActionResult> UploadPicture([FromForm(Name = "picture")] IFormFile picture,
        CancellationToken cancellationToken)
    {
        return SendAsync(new UploadAttendeePictureCommand
            {
                Picture = new FileProxy(picture)
            }, cancellationToken)
            .NoContent(HttpContext);
    }

    [HttpGet]
    public Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        return SendAsync(new GetAttendeeProfileQuery(), cancellationToken)
            .Ok(HttpContext);
    }

    [HttpGet("bookings")]
    public Task<IActionResult> GetBookings([FromQuery] GetAttendeeBookingsRequest request,
        CancellationToken cancellationToken)
    {
        return SendAsync(new GetAttendeeBookingsQuery
            {
                Page = request.Page,
                Limit = request.Limit
            }, cancellationToken)
            .Ok(HttpContext);
    }

    [HttpPost]
    public Task<IActionResult> UpdateProfile(UpdateAttendeeProfileRequest request,
        CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<UpdateAttendeeProfileCommand>(), cancellationToken)
            .NoContent(HttpContext);
    }

    [HttpDelete("picture")]
    public Task<IActionResult> RemovePicture(CancellationToken cancellationToken)
    {
        return SendAsync(new RemoveAttendeePictureCommand(), cancellationToken)
            .NoContent(HttpContext);
    }
}