using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

[Table("SubscriptionPayment")]
public class SubscriptionPayment
{
    [Key]
    [Column("IdSubscriptionPayment")]
    public int IdSubscriptionPayment { get; set; }

    [Required]
    [ForeignKey(nameof(Subscription))]
    [Column("IdSubscription")]
    public int IdSubscription { get; set; }

    public Subscription Subscription { get; set; }

    [Required]
    [Column("DateTime", TypeName = "datetime")]
    public DateTime DateTime { get; set; }
}