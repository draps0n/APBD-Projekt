using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

public class SubscriptionOffer
{
    [Key]
    public int IdSubscriptionOffer { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [ForeignKey(nameof(Software))]
    public int IdSoftware { get; set; }

    [Required]
    [Column("Price", TypeName = "money")]
    public decimal Price { get; set; }

    [Required]
    [ForeignKey(nameof(RenewalTime))]
    public int IdRenewalTime { get; set; }

    
    public Software Software { get; set; }
    public RenewalTime RenewalTime { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
}