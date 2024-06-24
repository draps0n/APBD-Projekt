using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

[Table("ContractPayment")]
public class ContractPayment
{
    [Key]
    [Column("ContractPayment")]
    public int IdContractPayment { get; set; }

    [Required]
    [ForeignKey(nameof(Contract))]
    [Column("IdContract")]
    public int IdContract { get; set; }

    public Contract Contract { get; set; }

    [Required]
    [Column("PaymentAmount", TypeName = "money")]
    public decimal PaymentAmount { get; set; }

    [Required]
    [Column("DateTime", TypeName = "datetime")]
    public DateTime DateTime { get; set; }
}