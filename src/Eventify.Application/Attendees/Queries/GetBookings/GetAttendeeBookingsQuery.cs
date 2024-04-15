using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Contracts.Attendees.Responses;
using Eventify.Contracts.Common.Responses;

namespace Eventify.Application.Attendees.Queries.GetBookings;

public sealed record GetAttendeeBookingsQuery : IQuery<PageResponse<AttendeeBookingResponse>>
{
    public int Page { get; init; }
    
    public int Limit { get; init; }
}