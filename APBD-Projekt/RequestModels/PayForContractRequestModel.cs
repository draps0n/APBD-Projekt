using System.ComponentModel.DataAnnotations;

namespace APBD_Projekt.RequestModels;

public class PayForContractRequestModel
{
    [Required]
    public decimal Amount { get; set; }
}