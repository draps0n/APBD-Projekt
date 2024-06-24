using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Context.Configurations;

public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(e => e.IdRole);
        builder.Property(e => e.IdRole)
            .ValueGeneratedOnAdd()
            .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

        builder.Property(e => e.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.HasMany(e => e.Users)
            .WithOne(e => e.Role)
            .HasForeignKey(e => e.IdRole)
            .IsRequired();
    }
}