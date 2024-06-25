using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using APBD_Projekt.Enums;
using APBD_Projekt.RequestModels;

namespace APBD_Projekt.Models;

public abstract class Client
{
    [Key]
    public int IdClient { get; protected set; }

    [Required]
    [MaxLength(250)]
    public string Address { get; protected set; }

    [Required]
    [EmailAddress]
    [MaxLength(250)]
    public string Email { get; protected set; }

    [Required]
    [Phone]
    [MaxLength(9)]
    public string Phone { get; protected set; }


    public ICollection<Contract> Contracts { get; private set; } = [];
    public ICollection<Subscription> Subscriptions { get; private set; } = [];

    protected Client() { }

    public Client(string address, string email, string phone)
    {
        Address = address;
        Email = email;
        Phone = phone;
    }

    public abstract void Delete();

    public virtual void Update(UpdateClientRequestModel requestModel)
    {
        Address = requestModel.Address;
        Email = requestModel.Email;
        Phone = requestModel.Phone;
    }
    public abstract void EnsureIsOfType(ClientType clientType);
}