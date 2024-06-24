using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Context.Configurations;

public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(e => e.IdCategory);
        builder.Property(e => e.IdCategory)
            .ValueGeneratedOnAdd()
            .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

        builder.Property(e => e.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasMany(e => e.Software)
            .WithMany(e => e.Categories);
    }
}