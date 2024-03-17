using Eventify.Domain.Common;
using Eventify.Domain.Common.Enums;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Enums;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Producers.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventify.Infrastructure.Events.Data.Configurations;

internal sealed class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        ConfigureEventTable(builder);
        ConfigureEventTicketsTable(builder);
        ConfigureEventBookingsTable(builder);
    }

    private static void ConfigureEventTable(EntityTypeBuilder<Event> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasConversion(v => v.Value, v => new EventId(v));

        builder.Property(e => e.ProducerId)
            .HasConversion(v => v.Value, v => new ProducerId(v));

        builder.Property(e => e.Slug)
            .HasColumnName("slug")
            .HasMaxLength(CommonConstants.SlugMaxLength)
            .HasConversion(v => v.Value, v => new Slug(v));

        builder.OwnsOne(e => e.Details, b =>
        {
            b.Property(d => d.Name)
                .HasColumnName("name")
                .HasMaxLength(EventConstants.NameMaxLength);

            b.Property(d => d.Description)
                .HasColumnName("description")
                .HasMaxLength(EventConstants.DescriptionMaxLength);

            b.Property(d => d.Category)
                .HasColumnName("category")
                .HasMaxLength(EventConstants.CategoryMaxLength)
                .HasConversion(v => v.Name, v => EventCategory.FromName(v, false));

            b.Property(d => d.Language)
                .HasColumnName("language")
                .HasMaxLength(EventConstants.LanguageMaxLength)
                .HasConversion(v => v.Name, v => Language.FromName(v, false));
        });

        builder.OwnsOne(e => e.Period, b =>
        {
            b.Property(s => s.Start)
                .HasColumnName("start")
                .HasColumnType("timestamp with time zone");

            b.Property(s => s.End)
                .HasColumnName("end")
                .HasColumnType("timestamp with time zone");
        });

        builder.Property(e => e.Location)
            .HasColumnType("jsonb");

        builder.Property(e => e.State)
            .HasMaxLength(EventConstants.StateMaxLength)
            .HasConversion(v => v.Name, v => EventState.FromName(v, false));

        builder.HasQueryFilter(e => e.IsDeleted == false);

        builder.ToTable("events");
    }

    private static void ConfigureEventTicketsTable(EntityTypeBuilder<Event> builder)
    {
        builder.OwnsMany(p => p.TicketIds, b =>
        {
            b.WithOwner()
                .HasForeignKey("event_id");

            b.Property(v => v.Value)
                .ValueGeneratedNever()
                .HasColumnName("ticket_id");

            b.HasKey("event_id", "Value");

            b.ToTable("event_tickets");
        });

        builder.Metadata.FindNavigation(nameof(Event.TicketIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureEventBookingsTable(EntityTypeBuilder<Event> builder)
    {
        builder.OwnsMany(p => p.BookingIds, b =>
        {
            b.WithOwner()
                .HasForeignKey("event_id");

            b.Property(v => v.Value)
                .ValueGeneratedNever()
                .HasColumnName("booking_id");

            b.HasKey("event_id", "Value");

            b.ToTable("event_bookings");
        });

        builder.Metadata.FindNavigation(nameof(Event.BookingIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}