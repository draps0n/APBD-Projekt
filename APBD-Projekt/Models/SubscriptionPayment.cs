namespace APBD_Projekt.Models;

public class SubscriptionPayment
{
    public int IdSubscriptionPayment { get; private set; }
    public int IdSubscription { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime DateTime { get; private set; }

    public Subscription Subscription { get; private set; }

    protected SubscriptionPayment()
    {
    }

    public SubscriptionPayment(decimal amount, DateTime dateTime, Subscription subscription)
    {
        Amount = amount;
        DateTime = dateTime;
        Subscription = subscription;
    }
}