using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Persistence.Configurations;

public class ContractEntityTypeConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.HasKey(c => c.IdContract);
        builder.Property(c => c.IdContract)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.StartDate)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(c => c.EndDate)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(c => c.YearsOfSupport)
            .IsRequired();

        builder.Property(c => c.FinalPrice)
            .HasColumnType("money")
            .IsRequired();

        builder.HasOne(c => c.Client)
            .WithMany(cl => cl.Contracts)
            .HasForeignKey(c => c.IdClient)
            .IsRequired();

        builder.HasOne(c => c.SoftwareVersion)
            .WithMany(sv => sv.Contracts)
            .HasForeignKey(c => c.IdSoftwareVersion)
            .IsRequired();

        builder.HasOne(c => c.Discount)
            .WithMany(d => d.Contracts)
            .HasForeignKey(c => c.IdDiscount)
            .IsRequired(false);

        builder.HasMany(c => c.ContractPayments)
            .WithOne(cp => cp.Contract)
            .HasForeignKey(cp => cp.IdContract)
            .IsRequired();
    }
}