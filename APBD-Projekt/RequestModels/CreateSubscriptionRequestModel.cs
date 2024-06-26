using System.ComponentModel.DataAnnotations;

namespace APBD_Projekt.RequestModels;

public class CreateSubscriptionRequestModel
{
    [Required]
    [MaxLength(100)]
    public string SubscriptionOfferName { get; set; }
    
    [Required]
    [MaxLength(150)]
    public string SoftwareName { get; set; }
}