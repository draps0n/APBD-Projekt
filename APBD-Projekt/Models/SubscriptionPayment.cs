namespace APBD_Projekt.Models;

public class SubscriptionPayment
{
    public int IdSubscriptionPayment { get; private set; }
    public int IdSubscription { get; private set; }
    public DateTime DateTime { get; private set; }

    public Subscription Subscription { get; private set; }

    protected SubscriptionPayment()
    {
    }

    public SubscriptionPayment(DateTime dateTime, Subscription subscription)
    {
        DateTime = dateTime;
        Subscription = subscription;
    }
}