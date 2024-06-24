using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

public class ContractPayment
{
    [Key]
    public int IdContractPayment { get; set; }

    [Required]
    [ForeignKey(nameof(Contract))]
    public int IdContract { get; set; }

    [Required]
    [Column("PaymentAmount", TypeName = "money")]
    public decimal PaymentAmount { get; set; }

    [Required]
    [Column("DateTime", TypeName = "datetime")]
    public DateTime DateTime { get; set; }
    
    
    public Contract Contract { get; set; }
}