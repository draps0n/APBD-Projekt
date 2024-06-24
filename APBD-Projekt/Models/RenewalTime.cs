using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

[Table("RenewalTime")]
public class RenewalTime
{
    [Key] [Column("IdRenewalTime")] public int IdRenewalTime { get; set; }

    [Required] [Column("Months")] public int Months { get; set; }

    [Required] [Column("Years")] public int Years { get; set; }

    public ICollection<SubscriptionOffer> SubscriptionOffers { get; set; }
}