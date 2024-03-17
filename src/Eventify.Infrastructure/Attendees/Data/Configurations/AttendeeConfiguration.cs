using Eventify.Domain.Attendees;
using Eventify.Domain.Attendees.ValueObjects;
using Eventify.Domain.Common;
using Eventify.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventify.Infrastructure.Attendees.Data.Configurations;

internal sealed class AttendeeConfiguration : IEntityTypeConfiguration<Attendee>
{
    public void Configure(EntityTypeBuilder<Attendee> builder)
    {
        ConfigureAttendeeTable(builder);
        ConfigureAttendeeBookingTable(builder);
    }

    private static void ConfigureAttendeeTable(EntityTypeBuilder<Attendee> builder)
    {
        builder.HasKey(a => a.Id);

        builder.HasIndex(a => a.UserId)
            .IsUnique();

        builder.Property(a => a.Id)
            .ValueGeneratedNever()
            .HasConversion(v => v.Value, v => new AttendeeId(v));

        builder.Property(a => a.UserId)
            .ValueGeneratedNever()
            .HasConversion(v => v.Value, v => new UserId(v))
            .HasMaxLength(CommonConstants.UserIdMaxLength);

        builder.Property(a => a.PictureUrl)
            .HasMaxLength(CommonConstants.UrlMaxLength);

        builder.OwnsOne(a => a.Details, b =>
        {
            b.Property(d => d.GivenName)
                .HasColumnName("given_name")
                .HasMaxLength(AttendeeConstants.GivenNameMaxLength);

            b.Property(d => d.FamilyName)
                .HasColumnName("family_name")
                .HasMaxLength(AttendeeConstants.FamilyNameMaxLength);

            b.Property(d => d.BirthDate)
                .HasColumnName("birth_date");
        });

        builder.OwnsOne(a => a.Contact, b =>
        {
            b.Property(c => c.Email)
                .HasColumnName("email")
                .HasMaxLength(CommonConstants.EmailMaxLength);

            b.Property(c => c.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(CommonConstants.PhoneNumberMaxLength);
        });

        builder.ToTable("attendees");
    }

    private static void ConfigureAttendeeBookingTable(EntityTypeBuilder<Attendee> builder)
    {
        builder.OwnsMany(p => p.BookingIds, b =>
        {
            b.WithOwner()
                .HasForeignKey("attendee_id");

            b.Property(v => v.Value)
                .ValueGeneratedNever()
                .HasColumnName("booking_id");

            b.HasKey("attendee_id", "Value");

            b.ToTable("attendee_bookings");
        });

        builder.Metadata.FindNavigation(nameof(Attendee.BookingIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}