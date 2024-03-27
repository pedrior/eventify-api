using Eventify.Contracts.Attendees.Responses;
using Eventify.Contracts.Common.Responses;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Tickets.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.Attendees.Queries.GetBookings;

internal sealed class GetAttendeeBookingsQueryHandler(
    IUser user,
    IAttendeeRepository attendeeRepository,
    IBookingRepository bookingRepository,
    IEventRepository eventRepository,
    ITicketRepository ticketRepository
) : IQueryHandler<GetAttendeeBookingsQuery, PageResponse<AttendeeBookingResponse>>
{
    public async Task<ErrorOr<PageResponse<AttendeeBookingResponse>>> Handle(GetAttendeeBookingsQuery query,
        CancellationToken cancellationToken)
    {
        var attendee = await attendeeRepository.GetByUserAsync(user.Id, cancellationToken);
        if (attendee is null)
        {
            throw new ApplicationException("Attendee not found");
        }

        var responses = new List<AttendeeBookingResponse>();
        foreach (var bookingId in attendee.BookingIds)
        {
            var booking = await bookingRepository.GetAsync(bookingId, cancellationToken);
            if (booking is null)
            {
                continue;
            }

            var @event = await eventRepository.GetAsync(booking.EventId, cancellationToken);
            var ticket = await ticketRepository.GetAsync(booking.TicketId, cancellationToken);
            
            if (@event is null || ticket is null)
            {
                continue;
            }
            
            responses.Add(new AttendeeBookingResponse
            {
                Id = booking.Id.Value,
                State = booking.State.Name,
                TotalPrice = booking.TotalPrice.Value,
                TotalQuantity = booking.TotalQuantity.Value,
                PlacedAt = booking.PlacedAt,
                Ticket = new AttendeeBookingTicketResponse
                {
                    Id = booking.TicketId.Value,
                    Name = ticket.Name,
                    Price = ticket.Price.Value
                },
                Event = new AttendeeBookingEventResponse
                {
                    Id = booking.EventId.Value,
                    Name = @event.Details.Name,
                    Start = @event.Period.Start,
                    End = @event.Period.End,
                    Location = @event.Location.ToString(),
                    PosterUrl = @event.PosterUrl?.ToString()
                }
            });
        }

        return new PageResponse<AttendeeBookingResponse>
        {
            Page = query.Page,
            Limit = query.Limit,
            Total = responses.Count,
            Items = responses
        };
    }
}