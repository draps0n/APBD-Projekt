using APBD_Projekt.Persistence;
using APBD_Projekt.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace APBD_Projekt.Repositories;

public class SubscriptionRepository(DatabaseContext context) : ISubscriptionsRepository
{
    public async Task<decimal> GetCurrentSubscriptionsRevenueAsync()
    {
        return await context.SubscriptionPayments
            .Include(subPay => subPay.Subscription)
            .ThenInclude(sub => sub.SubscriptionOffer)
            .SumAsync(subPay => subPay.Subscription.SubscriptionOffer.Price);
    }

    public async Task<decimal> GetCurrentSubscriptionsRevenueForSoftware(int softwareId)
    {
        return await context.SubscriptionPayments
            .Include(subPay => subPay.Subscription)
            .ThenInclude(sub => sub.SubscriptionOffer)
            .Where(subPay => subPay.Subscription.SubscriptionOffer.IdSoftware == softwareId)
            .SumAsync(subPay => subPay.Subscription.SubscriptionOffer.Price);
    }

    public async Task<decimal> GetNotYetPaidSubscriptionsRevenueAsync()
    {
        return await context.Subscriptions
            .Where(sub => sub.EndDate == null &&
                          sub.StartDate
                              .AddMonths(sub.SubscriptionOffer.MonthsPerRenewalTime * sub.SubscriptionPayments.Count) <
                          DateTime.Now &&
                          sub.StartDate <= DateTime.Now)
            .SumAsync(sub => sub.SubscriptionOffer.Price);
    }

    public async Task<decimal> GetNotYetPaidSubscriptionsRevenueForSoftwareAsync(int softwareId)
    {
        return await context.Subscriptions
            .Where(sub => sub.EndDate == null && sub.SubscriptionOffer.IdSoftware == softwareId &&
                          sub.StartDate
                              .AddMonths(sub.SubscriptionOffer.MonthsPerRenewalTime * sub.SubscriptionPayments.Count) <
                          DateTime.Now)
            .SumAsync(sub => sub.SubscriptionOffer.Price);
    }
}