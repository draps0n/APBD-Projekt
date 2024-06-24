using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

public class RenewalTime
{
    [Key]
    public int IdRenewalTime { get; set; }

    [Required]
    public int Months { get; set; }

    [Required]
    public int Years { get; set; }

    
    public ICollection<SubscriptionOffer> SubscriptionOffers { get; set; }
}