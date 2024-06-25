using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Persistence.Configurations;

public class RenewalTimeEntityTypeConfiguration : IEntityTypeConfiguration<RenewalTime>
{
    public void Configure(EntityTypeBuilder<RenewalTime> builder)
    {
        builder.HasKey(rt => rt.IdRenewalTime);
        builder.Property(rt => rt.IdRenewalTime)
            .ValueGeneratedOnAdd();

        builder.Property(rt => rt.Months)
            .IsRequired();
        
        builder.Property(rt => rt.Years)
            .IsRequired();

        builder.HasMany(rt => rt.SubscriptionOffers)
            .WithOne(subOff => subOff.RenewalTime)
            .HasForeignKey(subOff => subOff.IdRenewalTime)
            .IsRequired();
    }
}