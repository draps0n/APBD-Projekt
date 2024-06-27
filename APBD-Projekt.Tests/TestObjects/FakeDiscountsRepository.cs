using APBD_Projekt.Enums;
using APBD_Projekt.Models;
using APBD_Projekt.Repositories.Abstractions;

namespace APBD_Projekt.Tests.TestObjects;

public class FakeDiscountsRepository(List<Discount> discounts) : IDiscountsRepository
{
    public async Task<Discount?> GetBestActiveDiscountForContractAsync(DateTime startDate, DateTime endDate)
    {
        return discounts
            .Where(d => (d.Type == DiscountType.License || d.Type == DiscountType.Both) &&
                        d.StartDate <= endDate && d.EndDate >= startDate).MaxBy(d => d.Percentage);
    }

    public async Task<Discount?> GetBestActiveDiscountForSubscriptionAsync(DateTime currentDate)
    {
        return discounts
            .Where(d => (d.Type == DiscountType.Subscription || d.Type == DiscountType.Both) &&
                        currentDate > d.StartDate &&
                        currentDate < d.EndDate).MaxBy(d => d.Percentage);
    }
}