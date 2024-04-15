using Eventify.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Eventify.Infrastructure.Common.Persistence.Interceptors;

internal sealed class SoftDeleteInterceptor(TimeProvider timeProvider) : SaveChangesInterceptor
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

        foreach (var entry in data.Context.ChangeTracker.Entries<ISoftDelete>())
        {
            if (entry.State is not EntityState.Deleted)
            {
                continue;
            }

            entry.State = EntityState.Modified;

            // Update the state of owned entities to Modified so they are not deleted.
            // NOTE: this not apply to collection navigation properties.
            var ownedEntries = entry.References
                .Where(r => r.TargetEntry is { State: EntityState.Deleted }
                            && r.TargetEntry.Metadata.IsOwned())
                .Select(r => r.TargetEntry!);

            foreach (var ownerEntry in ownedEntries)
            {
                ownerEntry.State = EntityState.Modified;
            }

            entry.Entity.IsDeleted = true;
            entry.Entity.DeletedAt = time;
        }

        return base.SavingChangesAsync(data, result, cancellationToken);
    }
}