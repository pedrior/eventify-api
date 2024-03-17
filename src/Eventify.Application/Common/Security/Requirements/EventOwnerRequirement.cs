using Eventify.Application.Common.Abstractions.Security;
using Eventify.Domain.Events.ValueObjects;

namespace Eventify.Application.Common.Security.Requirements;

internal sealed record EventOwnerRequirement(EventId EventId) : IRequirement;