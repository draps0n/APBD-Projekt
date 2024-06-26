using APBD_Projekt.Enums;
using APBD_Projekt.Models;
using APBD_Projekt.Persistence;
using APBD_Projekt.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace APBD_Projekt.Repositories;

public class DiscountsRepository(DatabaseContext context) : IDiscountsRepository
{
    public async Task<Discount?> GetBestActiveDiscountForContract(DateTime startDate, DateTime endDate)
    {
        return await context.Discounts // TODO : fix żeby działało w jakiejkolwiek chwili w okresie
            .Where(d => (d.Type == DiscountType.License || d.Type == DiscountType.Both) && startDate > d.StartDate &&
                        startDate < d.EndDate)
            .OrderByDescending(d => d.Percentage)
            .FirstOrDefaultAsync();
    }

    public async Task<Discount?> GetBestActiveDiscountForSubscription(DateTime currentDate)
    {
        return await context.Discounts
            .Where(d => (d.Type == DiscountType.Subscription || d.Type == DiscountType.Both) &&
                        currentDate > d.StartDate &&
                        currentDate < d.EndDate)
            .OrderByDescending(d => d.Percentage)
            .FirstOrDefaultAsync();
    }
}