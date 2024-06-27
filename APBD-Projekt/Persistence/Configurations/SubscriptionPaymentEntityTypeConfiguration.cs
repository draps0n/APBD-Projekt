using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Persistence.Configurations;

public class SubscriptionPaymentEntityTypeConfiguration : IEntityTypeConfiguration<SubscriptionPayment>
{
    public void Configure(EntityTypeBuilder<SubscriptionPayment> builder)
    {
        builder.HasKey(subPay => subPay.IdSubscriptionPayment);
        builder.Property(subPay => subPay.IdSubscriptionPayment)
            .ValueGeneratedOnAdd();

        builder.Property(subPay => subPay.DateTime)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(subPay => subPay.Amount)
            .HasColumnType("money")
            .IsRequired();

        builder.HasOne(subPay => subPay.Subscription)
            .WithMany(sub => sub.SubscriptionPayments)
            .HasForeignKey(subPay => subPay.IdSubscription)
            .IsRequired();
    }
}