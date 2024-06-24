using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Context.Configurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.IdUser);
        builder.Property(e => e.IdUser)
            .ValueGeneratedOnAdd()
            .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

        builder.Property(e => e.Login)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(e => e.Password)
            .HasMaxLength(60)
            .IsRequired();
        
        builder.Property(e => e.Salt)
            .HasMaxLength(30)
            .IsRequired();

        builder.HasOne(e => e.Role)
            .WithMany(e => e.Users)
            .HasForeignKey(e => e.IdRole)
            .IsRequired();
        
        builder.Property(e => e.RefreshToken)
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(e => e.RefreshTokenExp)
            .HasColumnType("datetime");
    }
}