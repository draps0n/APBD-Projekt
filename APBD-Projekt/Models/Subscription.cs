using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

[Table("Subscription")]
public class Subscription
{
    [Key]
    [Column("IdSubscription")]
    public int IdSubscription { get; set; }
    
    [Required]
    [ForeignKey(nameof(Client))]
    [Column("IdClient")]
    public int IdClient { get; set; }

    public Client Client { get; set; }

    [Required]
    [ForeignKey(nameof(SubscriptionOffer))]
    [Column("IdSubscriptionOffer")]
    public int IdSubscriptionOffer { get; set; }

    public SubscriptionOffer SubscriptionOffer { get; set; }

    [Required]
    [Column("StartDate", TypeName = "datetime")]
    public DateTime StartDate { get; set; }
    
    [Column("EndDate", TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [ForeignKey(nameof(Discount))]
    [Column("IdDiscount")]
    public int? IdDiscount { get; set; }

    public Discount? Discount { get; set; }

    public ICollection<SubscriptionPayment> SubscriptionPayments { get; set; }
}