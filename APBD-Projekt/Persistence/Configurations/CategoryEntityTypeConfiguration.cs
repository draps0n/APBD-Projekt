using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Persistence.Configurations;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(c => c.IdCategory);
        builder.Property(c => c.IdCategory)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasMany(c => c.Software)
            .WithMany(c => c.Categories);
    }
}