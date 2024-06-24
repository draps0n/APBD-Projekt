using System.ComponentModel.DataAnnotations;

namespace APBD_Projekt.RequestModels;

public class RegisterUserRequestModel
{
    [Required]
    [MaxLength(50)]
    public string Login { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Password { get; set; }
}