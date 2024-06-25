using System.ComponentModel.DataAnnotations;

namespace APBD_Projekt.RequestModels;

public class UpdateClientRequestModel
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
    [MaxLength(20)]
    public string ClientType { get; set; }

    [MaxLength(50)]
    public string? Name { get; set; }

    [MaxLength(50)]
    public string? LastName { get; set; }

    [MaxLength(100)]
    public string? CompanyName { get; set; }
}