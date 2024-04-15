using Eventify.Domain.Common.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Eventify.Infrastructure.Common.Persistence.Interceptors;

internal sealed class EventDispatchInterceptor(IPublisher publisher) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData data,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (data.Context is null)
        {
            return await base.SavingChangesAsync(data, result, cancellationToken);
        }

        var entities = data.Context.ChangeTracker
            .Entries<IEntity>()
            .Where(e => e.Entity.DomainEvents.Count is not 0)
            .Select(e => e.Entity)
            .ToList();

        var events = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var @event in events)
        {
            await publisher.Publish(@event, cancellationToken);
        }

        return await base.SavingChangesAsync(data, result, cancellationToken);
    }
}