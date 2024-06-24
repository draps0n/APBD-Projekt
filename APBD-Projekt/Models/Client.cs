using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

[Table("Client")]
public abstract class Client
{
    [Key] public int IdClient { get; set; }

    [Required]
    [MaxLength(250)]
    [Column("Address")]
    public string Address { get; set; }

    [Required]
    [EmailAddress]
    [Column("Email")]
    public string Email { get; set; }

    [Required] [Phone] [Column("Phone")] public string Phone { get; set; }

    public ICollection<Contract> Contracts { get; set; }

    public ICollection<Subscription> Subscriptions { get; set; }
}