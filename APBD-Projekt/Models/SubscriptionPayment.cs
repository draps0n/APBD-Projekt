using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

public class SubscriptionPayment
{
    [Key]
    public int IdSubscriptionPayment { get; set; }

    [Required]
    [ForeignKey(nameof(Subscription))]
    public int IdSubscription { get; set; }

    [Required]
    [Column("DateTime", TypeName = "datetime")]
    public DateTime DateTime { get; set; }
    
    
    public Subscription Subscription { get; set; }
}