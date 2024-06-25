namespace APBD_Projekt.Models;

public class RenewalTime
{
    public int IdRenewalTime { get; private set; }
    public int Months { get; private set; }
    public int Years { get; private set; }

    public ICollection<SubscriptionOffer> SubscriptionOffers { get; private set; }

    protected RenewalTime()
    {
    }

    public RenewalTime(int months, int years)
    {
        Months = months;
        Years = years;
    }
}