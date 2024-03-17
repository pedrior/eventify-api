using Eventify.Domain.Common;
using Eventify.Domain.Producers;
using Eventify.Domain.Producers.ValueObjects;
using Eventify.Domain.Users.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Eventify.Infrastructure.Producers.Data.Configurations;

internal sealed class ProducerConfiguration : IEntityTypeConfiguration<Producer>
{
    public void Configure(EntityTypeBuilder<Producer> builder)
    {
        ConfigureProducerTable(builder);
        ConfigureProducerEventsTable(builder);
        ConfigureProducerDeletedEventsTable(builder);
    }

    private static void ConfigureProducerTable(EntityTypeBuilder<Producer> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasIndex(p => p.UserId)
            .IsUnique();

        builder.Property(p => p.Id)
            .ValueGeneratedNever()
            .HasConversion(v => v.Value, v => new ProducerId(v));

        builder.Property(p => p.UserId)
            .ValueGeneratedNever()
            .HasConversion(v => v.Value, v => new UserId(v))
            .HasMaxLength(CommonConstants.UserIdMaxLength);

        builder.Property(a => a.PictureUrl)
            .HasMaxLength(CommonConstants.UrlMaxLength);

        builder.Property(a => a.WebsiteUrl)
            .HasMaxLength(CommonConstants.UrlMaxLength);

        builder.OwnsOne(a => a.Details, b =>
        {
            b.Property(d => d.Name)
                .HasColumnName("given_name")
                .HasMaxLength(ProducerConstants.NameMaxLength);

            b.Property(d => d.Bio)
                .HasColumnName("family_name")
                .HasMaxLength(ProducerConstants.BioMaxLength);
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

        builder.ToTable("producers");
    }

    private static void ConfigureProducerEventsTable(EntityTypeBuilder<Producer> builder)
    {
        builder.OwnsMany(p => p.EventIds, b =>
        {
            b.WithOwner()
                .HasForeignKey("producer_id");

            b.Property(v => v.Value)
                .ValueGeneratedNever()
                .HasColumnName("event_id");

            b.HasKey("producer_id", "Value");

            b.ToTable("producer_events");
        });

        builder.Metadata.FindNavigation(nameof(Producer.EventIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }

    private static void ConfigureProducerDeletedEventsTable(EntityTypeBuilder<Producer> builder)
    {
        builder.OwnsMany(p => p.DeletedEventIds, b =>
        {
            b.WithOwner()
                .HasForeignKey("producer_id");

            b.Property(v => v.Value)
                .ValueGeneratedNever()
                .HasColumnName("event_id");

            b.HasKey("producer_id", "Value");

            b.ToTable("producer_deleted_events");
        });

        builder.Metadata.FindNavigation(nameof(Producer.DeletedEventIds))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}