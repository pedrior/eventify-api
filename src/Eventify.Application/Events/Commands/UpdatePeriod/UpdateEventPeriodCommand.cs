using Eventify.Application.Common.Abstractions.Data;

namespace Eventify.Application.Events.Commands.UpdatePeriod;

public sealed record UpdateEventPeriodCommand : ICommand<Updated>, ITransactional
{
    public required Guid EventId { get; init; }
    
    public required DateTimeOffset Start { get; init; }
    
    public required DateTimeOffset End { get; init; }
}