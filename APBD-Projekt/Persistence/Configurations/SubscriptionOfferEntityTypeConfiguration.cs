using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Persistence.Configurations;

public class SubscriptionOfferEntityTypeConfiguration : IEntityTypeConfiguration<SubscriptionOffer>
{
    public void Configure(EntityTypeBuilder<SubscriptionOffer> builder)
    {
        builder.HasKey(subOff => subOff.IdSubscriptionOffer);
        builder.Property(subOff => subOff.IdSubscriptionOffer)
            .ValueGeneratedOnAdd();

        builder.Property(subOff => subOff.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(subOff => subOff.Price)
            .HasColumnType("money")
            .IsRequired();

        builder.HasOne(subOff => subOff.Software)
            .WithMany(s => s.SubscriptionOffers)
            .HasForeignKey(subOff => subOff.IdSoftware)
            .IsRequired();

        builder.HasOne(subOff => subOff.RenewalTime)
            .WithMany(rt => rt.SubscriptionOffers)
            .HasForeignKey(subOff => subOff.IdRenewalTime)
            .IsRequired();

        builder.HasMany(subOff => subOff.Subscriptions)
            .WithOne(sub => sub.SubscriptionOffer)
            .HasForeignKey(sub => sub.IdSubscriptionOffer)
            .IsRequired();
    }
}