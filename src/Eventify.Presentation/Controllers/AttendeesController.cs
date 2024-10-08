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
    [HttpPost("profile-picture")]
    [RequestSizeLimit(RequestConstants.ImageSizeLimit)]
    public Task<IActionResult> UploadPicture([FromForm(Name = "picture")] IFormFile picture,
        CancellationToken cancellationToken)
    {
        return SendAsync(new UploadAttendeePictureCommand
            {
                Picture = new FileProxy(picture)
            }, cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }

    [HttpGet("profile")]
    public Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        return SendAsync(new GetAttendeeProfileQuery(), cancellationToken)
            .ToResponseAsync(Ok, HttpContext);
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
            .ToResponseAsync(Ok, HttpContext);
    }

    [HttpPost("update-profile")]
    public Task<IActionResult> UpdateProfile(UpdateAttendeeProfileRequest request,
        CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<UpdateAttendeeProfileCommand>(), cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }

    [HttpDelete("profile-picture")]
    public Task<IActionResult> RemovePicture(CancellationToken cancellationToken)
    {
        return SendAsync(new RemoveAttendeePictureCommand(), cancellationToken)
            .ToResponseAsync(_ => NoContent(), HttpContext);
    }
}