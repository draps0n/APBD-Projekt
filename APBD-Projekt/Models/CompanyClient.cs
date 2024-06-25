using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBD_Projekt.Models;

public class CompanyClient : Client
{
    [Required]
    [MaxLength(100)]
    public string CompanyName { get; private set; }

    [Required]
    [MaxLength(10)]
    public string KRS { get; private set; }

    protected CompanyClient() { }

    public CompanyClient(string address, string email, string phone, string companyName, string krs) : base(address, email, phone)
    {
        CompanyName = companyName;
        KRS = krs;
    }
}