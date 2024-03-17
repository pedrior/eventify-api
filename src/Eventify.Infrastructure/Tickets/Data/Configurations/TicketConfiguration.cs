using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventify.Infrastructure.Tickets.Data.Configurations;

internal sealed class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        ConfigureTicketTable(builder);
    }

    private static void ConfigureTicketTable(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedNever()
            .HasConversion(v => v.Value, v => new TicketId(v));

        builder.Property(t => t.EventId)
            .ValueGeneratedNever()
            .HasConversion(v => v.Value, v => new EventId(v));

        builder.Property(t => t.Name)
            .HasMaxLength(TicketConstants.NameMaxLength);

        builder.Property(t => t.Description)
            .HasMaxLength(TicketConstants.DescriptionMaxLength);

        builder.Property(t => t.Quantity)
            .HasConversion(v => v.Value, v => new Quantity(v));

        builder.Property(t => t.QuantityPerSale)
            .HasConversion(v => v.Value, v => new Quantity(v));

        builder.Property(t => t.QuantitySold)
            .HasConversion(v => v.Value, v => new Quantity(v));

        builder.OwnsOne(t => t.Price)
            .Property(p => p.Value)
            .HasColumnName("price")
            .HasColumnType("decimal(18,2)");
        
        builder.HasOne<Event>()
            .WithMany()
            .HasForeignKey(t => t.EventId);

        builder.ToTable("tickets");
    }
}