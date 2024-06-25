using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Persistence.Configurations;

public class ClientEntityTypeConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.UseTphMappingStrategy(); // TODO : co najlepsze?

        builder.HasKey(cl => cl.IdClient);
        builder.Property(cl => cl.IdClient)
            .ValueGeneratedOnAdd();

        builder.Property(cl => cl.Address)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(cl => cl.Email)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(cl => cl.Phone)
            .HasMaxLength(9)
            .IsRequired();

        builder.HasMany(cl => cl.Contracts)
            .WithOne(c => c.Client)
            .HasForeignKey(c => c.IdClient)
            .IsRequired();

        builder.HasMany(cl => cl.Subscriptions)
            .WithOne(s => s.Client)
            .HasForeignKey(s => s.IdClient)
            .IsRequired();
    }
}