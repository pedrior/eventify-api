using Eventify.Application.Common.Abstractions.Security;
using Eventify.Domain.Bookings.ValueObjects;

namespace Eventify.Application.Common.Security.Requirements;

internal sealed record BookingOwnerRequirement(BookingId BookingId) : IRequirement;