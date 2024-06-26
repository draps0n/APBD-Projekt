namespace APBD_Projekt.ResponseModels;

public class CreateSubscriptionResponseModel
{
    public int IdSubscription { get; set; }
    public string SubscriptionOfferName { get; set; }
    public string SoftwareName { get; set; }
    public decimal Fee { get; set; }
    public int MonthsPerRenewalTime { get; set; }
}