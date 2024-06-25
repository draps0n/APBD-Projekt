using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Persistence.Configurations;

public class SoftwareVersionEntityTypeConfiguration : IEntityTypeConfiguration<SoftwareVersion>
{
    public void Configure(EntityTypeBuilder<SoftwareVersion> builder)
    {
        builder.HasKey(sv => sv.IdSoftwareVersion);
        builder.Property(sv => sv.IdSoftwareVersion)
            .ValueGeneratedOnAdd();

        builder.Property(sv => sv.Version)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasOne(sv => sv.Software)
            .WithMany(s => s.SoftwareVersions)
            .HasForeignKey(sv => sv.IdSoftware)
            .IsRequired();

        builder.HasMany(sv => sv.Contracts)
            .WithOne(c => c.SoftwareVersion)
            .HasForeignKey(c => c.IdSoftwareVersion)
            .IsRequired();
    }
}