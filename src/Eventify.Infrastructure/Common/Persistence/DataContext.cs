using Eventify.Application.Common.Abstractions.Persistence;
using Eventify.Domain.Attendees;
using Eventify.Domain.Events;
using Eventify.Domain.Producers;
using Microsoft.EntityFrameworkCore.Storage;

namespace Eventify.Infrastructure.Common.Persistence;

internal sealed class DataContext(DbContextOptions<DataContext> options) : DbContext(options), IUnitOfWork
{
    private IDbContextTransaction? transaction;

    public DbSet<Attendee> Attendees => Set<Attendee>();

    public DbSet<Event> Events => Set<Event>();
    
    public DbSet<Producer> Producers => Set<Producer>();

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
        
        builder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }

    public async Task<Guid> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (transaction is not null)
        {
            return transaction.TransactionId;
        }

        transaction = await Database.BeginTransactionAsync(cancellationToken);
        return transaction.TransactionId;
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (transaction is null)
        {
            return;
        }

        await transaction.CommitAsync(cancellationToken);
        await transaction.DisposeAsync();

        transaction = null;
    }
}