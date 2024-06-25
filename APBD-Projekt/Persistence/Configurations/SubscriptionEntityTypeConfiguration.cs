using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Persistence.Configurations;

public class SubscriptionEntityTypeConfiguration : IEntityTypeConfiguration<Subscription>
{
    public void Configure(EntityTypeBuilder<Subscription> builder)
    {
        builder.HasKey(sub => sub.IdSubscription);
        builder.Property(sub => sub.IdSubscription)
            .ValueGeneratedOnAdd();

        builder.Property(sub => sub.StartDate)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(sub => sub.EndDate)
            .HasColumnType("datetime");

        builder.HasOne(sub => sub.Client)
            .WithMany(cl => cl.Subscriptions)
            .HasForeignKey(sub => sub.IdClient)
            .IsRequired();

        builder.HasOne(sub => sub.SubscriptionOffer)
            .WithMany(subOff => subOff.Subscriptions)
            .HasForeignKey(sub => sub.IdSubscriptionOffer)
            .IsRequired();

        builder.HasOne(sub => sub.Discount)
            .WithMany(d => d.Subscriptions)
            .HasForeignKey(sub => sub.IdDiscount)
            .IsRequired(false);

        builder.HasMany(sub => sub.SubscriptionPayments)
            .WithOne(subPay => subPay.Subscription)
            .HasForeignKey(subPay => subPay.IdSubscription)
            .IsRequired();
    }
}