using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Persistence.Configurations;

public class SoftwareEntityTypeConfiguration : IEntityTypeConfiguration<Software>
{
    public void Configure(EntityTypeBuilder<Software> builder)
    {
        builder.HasKey(s => s.IdSoftware);
        builder.Property(s => s.IdSoftware)
            .ValueGeneratedOnAdd();

        builder.Property(s => s.Name)
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(s => s.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(s => s.YearlyLicensePrice)
            .HasColumnType("money")
            .IsRequired();

        builder.HasMany(s => s.Categories)
            .WithMany(s => s.Software);

        builder.HasMany(s => s.SoftwareVersions)
            .WithOne(sv => sv.Software)
            .HasForeignKey(sv => sv.IdSoftware)
            .IsRequired();

        builder.HasMany(s => s.SubscriptionOffers)
            .WithOne(subOff => subOff.Software)
            .HasForeignKey(subOff => subOff.IdSoftware)
            .IsRequired();
    }
}