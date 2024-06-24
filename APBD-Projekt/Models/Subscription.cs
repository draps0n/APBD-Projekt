using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

public class Subscription
{
    [Key]
    public int IdSubscription { get; set; }
    
    [Required]
    [ForeignKey(nameof(Client))]
    public int IdClient { get; set; }

    [Required]
    [ForeignKey(nameof(SubscriptionOffer))]
    public int IdSubscriptionOffer { get; set; }

    [Required]
    [Column("StartDate", TypeName = "datetime")]
    public DateTime StartDate { get; set; }
    
    [Column("EndDate", TypeName = "datetime")]
    public DateTime? EndDate { get; set; }

    [ForeignKey(nameof(Discount))]
    public int? IdDiscount { get; set; }

    
    public Client Client { get; set; }
    public SubscriptionOffer SubscriptionOffer { get; set; }
    public Discount? Discount { get; set; }
    public ICollection<SubscriptionPayment> SubscriptionPayments { get; set; }
}