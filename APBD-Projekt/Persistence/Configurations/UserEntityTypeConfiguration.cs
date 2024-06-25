using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Persistence.Configurations;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.IdUser);
        builder.Property(u => u.IdUser)
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Login)
            .HasMaxLength(50)
            .IsRequired();
        builder.HasIndex(u => u.Login)
            .IsUnique();

        builder.Property(u => u.Password)
            .HasMaxLength(60)
            .IsRequired();
        
        builder.Property(u => u.Salt)
            .HasMaxLength(30)
            .IsRequired();
        
        builder.Property(u => u.RefreshToken)
            .HasMaxLength(60)
            .IsRequired();

        builder.Property(u => u.RefreshTokenExp)
            .HasColumnType("datetime")
            .IsRequired();

        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.IdRole)
            .IsRequired();
    }
}