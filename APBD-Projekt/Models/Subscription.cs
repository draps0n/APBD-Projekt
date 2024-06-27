using APBD_Projekt.Exceptions;

namespace APBD_Projekt.Models;

public class Subscription
{
    public int IdSubscription { get; private set; }
    public int IdClient { get; private set; }
    public int IdSubscriptionOffer { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public int? IdDiscount { get; private set; }
    public DateTime NextPaymentDueDate { get; set; }
    public bool ShouldApplyRegularClientDiscount { get; set; }

    public Client Client { get; private set; }
    public SubscriptionOffer SubscriptionOffer { get; private set; }
    public Discount? Discount { get; private set; }
    public ICollection<SubscriptionPayment> SubscriptionPayments { get; private set; } = [];

    protected Subscription()
    {
    }

    public Subscription(DateTime startDate, Client client, SubscriptionOffer subscriptionOffer,
        Discount? discount = null)
    {
        ShouldApplyRegularClientDiscount = client.IsRegularClient();
        StartDate = startDate;
        Client = client;
        SubscriptionOffer = subscriptionOffer;
        Discount = discount;
        SubscriptionPayments = [];
    }

    public void ProcessFirstPayment()
    {
        var firstPaymentFee = CalculateFee();
        if (Discount != null)
        {
            firstPaymentFee *= 1 - (decimal)Discount.Percentage / 100;
        }

        var firstPayment = new SubscriptionPayment(firstPaymentFee, StartDate, this);
        SubscriptionPayments.Add(firstPayment);
        NextPaymentDueDate = StartDate.AddMonths(SubscriptionOffer.MonthsPerRenewalTime * 2);
    }

    public bool IsActiveAndForGivenSoftware(int softwareId)
    {
        return SubscriptionOffer.IdSoftware == softwareId && EndDate == null;
    }

    public decimal CalculateFee()
    {
        return ShouldApplyRegularClientDiscount ? SubscriptionOffer.Price * (decimal)0.95 : SubscriptionOffer.Price;
    }

    private void EnsurePaymentIsNotLate()
    {
        if (NextPaymentDueDate >= DateTime.Now)
        {
            return;
        }

        EndDate = DateTime.Now;
        throw new BadRequestException(
            "Previous subscription fees were not paid on time. Subscription is cancelled");
    }

    private void EnsurePaymentIsNotAlreadyPaid()
    {
        if (DateTime.Now.AddMonths(SubscriptionOffer.MonthsPerRenewalTime) < NextPaymentDueDate)
        {
            throw new BadRequestException("Current subscription period is already paid");
        }
    }

    private void EnsurePaymentAmountIsValid(decimal amount)
    {
        if (amount != CalculateFee())
        {
            throw new BadRequestException($"Subscription fee equals: {CalculateFee()}. Exact amount has to be paid");
        }
    }

    public void ProcessPayment(decimal amount)
    {
        EnsurePaymentIsNotLate();
        EnsurePaymentIsNotAlreadyPaid();
        EnsurePaymentAmountIsValid(amount);

        var payment = new SubscriptionPayment(CalculateFee(), DateTime.Now, this);
        SubscriptionPayments.Add(payment);
        NextPaymentDueDate = NextPaymentDueDate.AddMonths(SubscriptionOffer.MonthsPerRenewalTime);
    }
}