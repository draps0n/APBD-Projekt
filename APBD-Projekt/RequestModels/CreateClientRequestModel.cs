using System.ComponentModel.DataAnnotations;
using APBD_Projekt.Enums;

namespace APBD_Projekt.RequestModels;

public class CreateClientRequestModel
{
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

    [Required]
    public ClientType ClientType { get; set; }

    [MaxLength(50)]
    public string? Name { get; set; }

    [MaxLength(50)]
    public string? LastName { get; set; }

    [MaxLength(11)]
    public string? PESEL { get; set; }

    [MaxLength(100)]
    public string? CompanyName { get; set; }

    [MaxLength(10)]
    public string? KRS { get; set; }
}
