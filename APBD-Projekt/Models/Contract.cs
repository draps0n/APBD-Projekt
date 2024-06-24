using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

public class Contract
{
    [Key]
    public int IdContract { get; set; }

    [Required]
    [ForeignKey(nameof(Client))]
    public int IdClient { get; set; }

    [Required]
    [ForeignKey(nameof(SoftwareVersion))]
    public int IdSoftwareVersion { get; set; }

    [Required]
    [Column("StartDate", TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Required]
    [Column("EndDate", TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [Required]
    public int YearsOfSupport { get; set; }

    [Required]
    [Column("FinalPrice", TypeName = "money")]
    public decimal FinalPrice { get; set; }

    [ForeignKey(nameof(Discount))]
    public int? IdDiscount { get; set; }
    
    
    public Client Client { get; set; }
    public SoftwareVersion SoftwareVersion { get; set; }
    public Discount? Discount { get; set; }
    public ICollection<ContractPayment> ContractPayments { get; set; }
}