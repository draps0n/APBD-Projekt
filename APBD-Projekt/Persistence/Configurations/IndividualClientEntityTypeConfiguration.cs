using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Persistence.Configurations;

public class IndividualClientEntityTypeConfiguration : IEntityTypeConfiguration<IndividualClient>
{
    public void Configure(EntityTypeBuilder<IndividualClient> builder)
    {
        builder.Property(icl => icl.Name)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(icl => icl.LastName)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(icl => icl.PESEL)
            .HasMaxLength(11)
            .IsRequired();

        builder.Property(icl => icl.IsDeleted)
            .IsRequired();
    }
}