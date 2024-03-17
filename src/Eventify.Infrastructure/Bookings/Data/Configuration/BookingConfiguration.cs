using Eventify.Domain.Attendees.ValueObjects;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Enums;
using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventify.Infrastructure.Bookings.Data.Configuration;

internal sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .ValueGeneratedNever()
            .HasConversion(v => v.Value, v => new BookingId(v));

        builder.Property(b => b.TicketId)
            .ValueGeneratedNever()
            .HasConversion(v => v.Value, v => new TicketId(v));

        builder.Property(b => b.EventId)
            .ValueGeneratedNever()
            .HasConversion(v => v.Value, v => new EventId(v));

        builder.Property(b => b.AttendeeId)
            .ValueGeneratedNever()
            .HasConversion(v => v.Value, v => new AttendeeId(v));

        builder.Property(b => b.TotalPrice)
            .HasColumnType("decimal(18,2)")
            .HasConversion(v => v.Value, v => new Money(v));

        builder.Property(b => b.TotalQuantity)
            .HasConversion(v => v.Value, v => new Quantity(v));

        builder.Property(b => b.State)
            .HasMaxLength(BookingConstants.StateMaxLength)
            .HasConversion(v => v.Name, v => BookingState.FromName(v, false));

        builder.Property(b => b.CancellationReason)
            .HasMaxLength(BookingConstants.CancellationReasonMaxLength)
            .HasConversion(
                v => v == null ? null : v.Name,
                v => v == null ? null : CancellationReason.FromName(v, false));

        builder.ToTable("bookings");
    }
}