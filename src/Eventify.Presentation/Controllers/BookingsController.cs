using Eventify.Application.Bookings.Commands.CancelBooking;
using Eventify.Application.Bookings.Commands.CreateBooking;
using Eventify.Application.Bookings.Queries.GetBooking;
using Eventify.Contracts.Bookings.Requests;

namespace Eventify.Presentation.Controllers;

public sealed class BookingsController : ApiController
{
    [HttpPost]
    public Task<IActionResult> CreateBooking(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<CreateBookingCommand>(), cancellationToken)
            .CreatedAtAction(
                actionName: nameof(GetBooking),
                routeValues: response => new { id = response.Id },
                context: HttpContext);
    }

    [HttpPost("{id:guid}/cancel")]
    public Task<IActionResult> CancelBooking(Guid id, CancellationToken cancellationToken)
    {
        return SendAsync(new CancelBookingCommand { BookingId = id }, cancellationToken)
            .NoContent(HttpContext);
    }

    [HttpGet("{id:guid}")]
    public  Task<IActionResult> GetBooking(Guid id, CancellationToken cancellationToken)
    {
        return SendAsync(new GetBookingQuery { BookingId = id }, cancellationToken)
            .Ok(HttpContext);
    }
}