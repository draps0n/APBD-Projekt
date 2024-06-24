using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

public class Discount
{
    public static readonly int LicenseOnlyType = 0;
    public static readonly int SubscriptionOnlyType = 1;
    public static readonly int BothType = 2;

    [Key]
    public int IdDiscount { get; set; }

    [Required]
    [MaxLength(250)]
    public string Name { get; set; }

    [Required]
    [Range(0, 2)]
    public int Type { get; set; }

    [Required]
    [Range(1, 99)]
    public int Percentage { get; set; }

    [Required]
    [Column("StartDate", TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Required]
    [Column("EndDate", TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    
    public ICollection<Contract> Contracts { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
}