using APBD_Projekt.Models;
using APBD_Projekt.Persistence;
using APBD_Projekt.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace APBD_Projekt.Repositories;

public class SubscriptionsRepository(DatabaseContext context) : ISubscriptionsRepository
{
    public async Task<decimal> GetCurrentSubscriptionsRevenueAsync()
    {
        return await context.SubscriptionPayments
            .SumAsync(subPay => subPay.Amount);
    }

    public async Task<decimal> GetCurrentSubscriptionsRevenueForSoftwareAsync(int softwareId)
    {
        return await context.SubscriptionPayments
            .Where(subPay => subPay.Subscription.SubscriptionOffer.IdSoftware == softwareId)
            .SumAsync(subPay => subPay.Amount);
    }

    public async Task<decimal> GetNotYetPaidSubscriptionsRevenueAsync()
    {
        return await context.Subscriptions
            .Where(sub => sub.EndDate == null &&
                          sub.StartDate
                              .AddMonths(sub.SubscriptionOffer.MonthsPerRenewalTime * sub.SubscriptionPayments.Count) <
                          DateTime.Now &&
                          sub.StartDate <= DateTime.Now)
            .SumAsync(sub => sub.SubscriptionOffer.Price * (decimal)(sub.ShouldApplyRegularClientDiscount ? 0.95 : 1));
    }

    public async Task<decimal> GetNotYetPaidSubscriptionsRevenueForSoftwareAsync(int softwareId)
    {
        return await context.Subscriptions
            .Where(sub => sub.EndDate == null && sub.SubscriptionOffer.IdSoftware == softwareId &&
                          sub.StartDate
                              .AddMonths(sub.SubscriptionOffer.MonthsPerRenewalTime * sub.SubscriptionPayments.Count) <
                          DateTime.Now)
            .SumAsync(sub => sub.SubscriptionOffer.Price * (decimal)(sub.ShouldApplyRegularClientDiscount ? 0.95 : 1));
    }

    public async Task CreateSubscriptionAsync(Subscription subscription)
    {
        await context.Subscriptions.AddAsync(subscription);
    }

    public async Task<Subscription?> GetSubscriptionByIdAsync(int subscriptionId)
    {
        return await context.Subscriptions
            .Include(sub => sub.SubscriptionOffer)
            .Where(sub => sub.IdSubscription == subscriptionId)
            .FirstOrDefaultAsync();
    }

    public void UpdateSubscription(Subscription subscription)
    {
        context.Subscriptions.Update(subscription);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}