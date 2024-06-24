using System.ComponentModel.DataAnnotations;

namespace APBD_Projekt.RequestModels;

public class RefreshTokenRequestModel
{
    [Required]
    [MaxLength(60)]
    public required string RefreshToken { get; set; }
}