using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

public abstract class Client
{
    [Key]
    public int IdClient { get; set; }

    [Required]
    [MaxLength(250)]
    public string Address { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(250)]
    public string Email { get; set; }

    [Required]
    [Phone]
    [MaxLength(9)]
    public string Phone { get; set; }

    
    public ICollection<Contract> Contracts { get; set; }
    public ICollection<Subscription> Subscriptions { get; set; }
}