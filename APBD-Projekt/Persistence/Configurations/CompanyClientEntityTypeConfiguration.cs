using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Persistence.Configurations;

public class CompanyClientEntityTypeConfiguration : IEntityTypeConfiguration<CompanyClient>
{
    public void Configure(EntityTypeBuilder<CompanyClient> builder)
    {
        builder.Property(ccl => ccl.CompanyName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(ccl => ccl.KRS)
            .HasMaxLength(10)
            .IsRequired();
    }
}