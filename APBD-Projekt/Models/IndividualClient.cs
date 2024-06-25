using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

public class IndividualClient : Client
{
    [Required]
    [MaxLength(50)]
    public string Name { get; private set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; private set; }

    [Required]
    [MaxLength(11)]
    public string PESEL { get; private set; }

    protected IndividualClient() { }

    public IndividualClient(string address, string email, string phone, string name, string lastName, string pesel) : base(address, email, phone)
    {
        Name = name;
        LastName = lastName;
        PESEL = pesel;
    }

    public override void Delete()
    {
        Address = String.Empty;
        Email = String.Empty;
        Phone = String.Empty;
        Name = String.Empty;
        LastName = String.Empty;
        PESEL = String.Empty;
    }
}