﻿using Eventify.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Eventify.Infrastructure.Common.Persistence.Interceptors;

internal sealed class AuditableInterceptor(TimeProvider timeProvider) : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData data,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (data.Context is null)
        {
            return base.SavingChangesAsync(data, result, cancellationToken);
        }

        var time = timeProvider.GetUtcNow();

        foreach (var entry in data.Context.ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State is EntityState.Added)
            {
                entry.Entity.CreatedAt = time;
            }

            if (entry.State is EntityState.Modified || HasChangedOwnedEntities(entry))
            {
                entry.Entity.UpdatedAt = time;
            }
        }
        
        return base.SavingChangesAsync(data, result, cancellationToken);
    }

    private static bool HasChangedOwnedEntities(EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry is { State: EntityState.Added or EntityState.Modified } &&
            r.TargetEntry.Metadata.IsOwned());
}