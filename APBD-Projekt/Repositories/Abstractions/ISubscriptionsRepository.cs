using APBD_Projekt.Models;

namespace APBD_Projekt.Repositories.Abstractions;

public interface ISubscriptionsRepository
{
    Task<decimal> GetCurrentSubscriptionsRevenueAsync();
    Task<decimal> GetCurrentSubscriptionsRevenueForSoftware(int softwareId);
    Task<decimal> GetNotYetPaidSubscriptionsRevenueAsync();
    Task<decimal> GetNotYetPaidSubscriptionsRevenueForSoftwareAsync(int softwareId);
    Task CreateSubscriptionAsync(Subscription subscription);
    Task<Subscription?> GetSubscriptionByIdAsync(int subscriptionId);
    Task UpdateSubscription(Subscription subscription);
}