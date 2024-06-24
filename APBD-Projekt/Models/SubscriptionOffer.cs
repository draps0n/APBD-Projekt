using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

[Table("SubscriptionOffer")]
public class SubscriptionOffer
{
    [Key] [Column("IdSubscriptionOffer")] public int IdSubscriptionOffer { get; set; }

    [Required]
    [Column("Name")]
    [MaxLength(100)]
    public string Name { get; set; }

    [Required]
    [ForeignKey(nameof(Software))]
    [Column("IdSoftware")]
    public int IdSoftware { get; set; }

    public Software Software { get; set; }

    [Required]
    [Column("Price", TypeName = "money")]
    public decimal Price { get; set; }

    [Required]
    [ForeignKey(nameof(RenewalTime))]
    [Column("IdRenewalTime")]
    public int IdRenewalTime { get; set; }

    public RenewalTime RenewalTime { get; set; }

    public ICollection<Subscription> Subscriptions { get; set; }
}