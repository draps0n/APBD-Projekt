using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

public class Software
{
    [Key]
    public int IdSoftware { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; set; }

    [Required]
    [Column("YearlyLicensePrice", TypeName = "money")]
    public decimal YearlyLicensePrice { get; set; }

    
    public ICollection<Category> Categories { get; set; }
    public ICollection<SoftwareVersion> SoftwareVersions { get; set; }
    public ICollection<SubscriptionOffer> SubscriptionOffers { get; set; }
}