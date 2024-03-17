using Eventify.Application.Bookings.Commands.CancelBooking;
using Eventify.Application.Bookings.Commands.CreateBooking;
using Eventify.Application.Bookings.Queries.GetBooking;
using Eventify.Contracts.Bookings.Requests;

namespace Eventify.Presentation.Controllers;

public sealed class BookingsController : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateBooking(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var result = await SendAsync(request.Adapt<CreateBookingCommand>(), cancellationToken);
        return result.Match(
            onValue: response => CreatedAtAction(nameof(GetBooking), new { response.Id }, response),
            onError: Problem);
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> CancelBooking(Guid id, CancellationToken cancellationToken)
    {
        var result = await SendAsync(new CancelBookingCommand { BookingId = id }, cancellationToken);
        return result.Match(onValue: _ => NoContent(), onError: Problem);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetBooking(Guid id, CancellationToken cancellationToken)
    {
        var result = await SendAsync(new GetBookingQuery { BookingId = id }, cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }
}