namespace APBD_Projekt.Models;

public class Subscription
{
    public int IdSubscription { get; private set; }
    public int IdClient { get; private set; }
    public int IdSubscriptionOffer { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public int? IdDiscount { get; private set; }

    public Client Client { get; private set; }
    public SubscriptionOffer SubscriptionOffer { get; private set; }
    public Discount? Discount { get; private set; }
    public ICollection<SubscriptionPayment> SubscriptionPayments { get; private set; }

    protected Subscription()
    {
    }

    public Subscription(DateTime startDate, DateTime? endDate, Client client, SubscriptionOffer subscriptionOffer,
        Discount? discount = null)
    {
        StartDate = startDate;
        EndDate = endDate;
        Client = client;
        SubscriptionOffer = subscriptionOffer;
        Discount = discount;
    }
}