using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Persistence.Configurations;

public class DiscountEntityTypeConfiguration : IEntityTypeConfiguration<Discount>
{
    public void Configure(EntityTypeBuilder<Discount> builder)
    {
        builder.HasKey(d => d.IdDiscount);
        builder.Property(d => d.IdDiscount)
            .ValueGeneratedOnAdd();

        builder.Property(d => d.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(d => d.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(d => d.Percentage)
            .IsRequired();

        builder.Property(d => d.StartDate)
            .HasColumnType("datetime")
            .IsRequired();
        
        builder.Property(d => d.EndDate)
            .HasColumnType("datetime")
            .IsRequired();

        builder.HasMany(d => d.Contracts)
            .WithOne(c => c.Discount)
            .HasForeignKey(c => c.IdDiscount)
            .IsRequired();
        
        builder.HasMany(d => d.Subscriptions)
            .WithOne(sub => sub.Discount)
            .HasForeignKey(sub => sub.IdDiscount)
            .IsRequired();
    }
}