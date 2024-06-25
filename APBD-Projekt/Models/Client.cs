using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

public abstract class Client
{
    [Key]
    public int IdClient { get; private set; }

    [Required]
    [MaxLength(250)]
    public string Address { get; private set; }

    [Required]
    [EmailAddress]
    [MaxLength(250)]
    public string Email { get; private set; }

    [Required]
    [Phone]
    [MaxLength(9)]
    public string Phone { get; private set; }


    public ICollection<Contract> Contracts { get; private set; } = [];
    public ICollection<Subscription> Subscriptions { get; private set; } = [];

    protected Client() { }

    public Client(string address, string email, string phone)
    {
        Address = address;
        Email = email;
        Phone = phone;
    }
}