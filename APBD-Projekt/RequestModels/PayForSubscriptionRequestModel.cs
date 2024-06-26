using System.ComponentModel.DataAnnotations;

namespace APBD_Projekt.RequestModels;

public class PayForSubscriptionRequestModel
{
    [Required]
    public decimal Amount { get; set; }
}