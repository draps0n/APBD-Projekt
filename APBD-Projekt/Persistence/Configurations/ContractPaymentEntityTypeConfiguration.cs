using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APBD_Projekt.Persistence.Configurations;

public class ContractPaymentEntityTypeConfiguration : IEntityTypeConfiguration<ContractPayment>
{
    public void Configure(EntityTypeBuilder<ContractPayment> builder)
    {
        builder.HasKey(cp => cp.IdContractPayment);
        builder.Property(cp => cp.IdContractPayment)
            .ValueGeneratedOnAdd();

        builder.Property(cp => cp.PaymentAmount)
            .HasColumnType("money")
            .IsRequired();

        builder.Property(cp => cp.DateTime)
            .HasColumnType("datetime")
            .IsRequired();

        builder.HasOne(cp => cp.Contract)
            .WithMany(c => c.ContractPayments)
            .HasForeignKey(cp => cp.IdContract)
            .IsRequired();
    }
}