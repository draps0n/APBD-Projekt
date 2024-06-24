using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

[Table("Contract")]
public class Contract
{
    [Key] [Column("IdContract")] public int IdContract { get; set; }

    [Required]
    [ForeignKey(nameof(Client))]
    [Column("IdClient")]
    public int IdClient { get; set; }

    public Client Client { get; set; }

    [Required]
    [ForeignKey(nameof(SoftwareVersion))]
    [Column("IdSoftwareVersion")]
    public int IdSoftwareVersion { get; set; }

    public SoftwareVersion SoftwareVersion { get; set; }

    [Required]
    [Column("StartDate", TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Required]
    [Column("EndDate", TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [Required] [Column("YearsOfSupport")] public int YearsOfSupport { get; set; }

    [Required]
    [Column("FinalPrice", TypeName = "money")]
    public decimal FinalPrice { get; set; }

    [ForeignKey(nameof(Discount))]
    [Column("IdDiscount")]
    public int? IdDiscount { get; set; }

    public Discount? Discount { get; set; }

    public ICollection<ContractPayment> ContractPayments { get; set; }
}