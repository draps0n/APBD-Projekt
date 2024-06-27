using APBD_Projekt.Models;
using APBD_Projekt.Repositories.Abstractions;

namespace APBD_Projekt.Tests.TestObjects;

public class FakeSubscriptionsRepository(
    List<Subscription> subscriptions,
    List<SubscriptionPayment> subscriptionPayments) : ISubscriptionsRepository
{
    public async Task<decimal> GetCurrentSubscriptionsRevenueAsync()
    {
        return subscriptionPayments
            .Sum(subPay => subPay.Amount);
    }

    public async Task<decimal> GetCurrentSubscriptionsRevenueForSoftwareAsync(int softwareId)
    {
        return subscriptionPayments
            .Where(subPay => subPay.Subscription.SubscriptionOffer.IdSoftware == softwareId)
            .Sum(subPay => subPay.Amount);
    }

    public async Task<decimal> GetNotYetPaidSubscriptionsRevenueAsync()
    {
        return subscriptions
            .Where(sub => sub.EndDate == null &&
                          sub.StartDate
                              .AddMonths(sub.SubscriptionOffer.MonthsPerRenewalTime * sub.SubscriptionPayments.Count) <
                          DateTime.Now &&
                          sub.StartDate <= DateTime.Now)
            .Sum(sub => sub.SubscriptionOffer.Price * (decimal)(sub.ShouldApplyRegularClientDiscount ? 0.95 : 1));
    }

    public async Task<decimal> GetNotYetPaidSubscriptionsRevenueForSoftwareAsync(int softwareId)
    {
        return subscriptions
            .Where(sub => sub.EndDate == null && sub.SubscriptionOffer.IdSoftware == softwareId &&
                          sub.StartDate
                              .AddMonths(sub.SubscriptionOffer.MonthsPerRenewalTime * sub.SubscriptionPayments.Count) <
                          DateTime.Now)
            .Sum(sub => sub.SubscriptionOffer.Price * (decimal)(sub.ShouldApplyRegularClientDiscount ? 0.95 : 1));
    }

    public async Task CreateSubscriptionAsync(Subscription subscription)
    {
        subscriptions.Add(subscription);
    }

    public async Task<Subscription?> GetSubscriptionWithOfferByIdAsync(int subscriptionId)
    {
        return subscriptions
            .FirstOrDefault(sub => sub.IdSubscription == subscriptionId);
    }

    public void UpdateSubscription(Subscription subscription)
    {
        var subscriptionInDb = subscriptions.FirstOrDefault(sub => sub.IdSubscription == subscription.IdSubscription)!;
        subscriptions.Remove(subscriptionInDb);
        subscriptions.Add(subscription);
    }

    public async Task SaveChangesAsync()
    {
    }
}