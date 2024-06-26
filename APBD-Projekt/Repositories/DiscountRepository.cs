using APBD_Projekt.Enums;
using APBD_Projekt.Models;
using APBD_Projekt.Persistence;
using APBD_Projekt.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace APBD_Projekt.Repositories;

public class DiscountRepository(DatabaseContext context) : IDiscountsRepository
{
    public async Task<Discount?> GetBestActiveDiscountForContract(DateTime startDate, DateTime endDate)
    {
        return await context.Discounts
            .Where(d => (d.Type == DiscountType.License || d.Type == DiscountType.Both) && startDate > d.StartDate &&
                        DateTime.Now < d.EndDate)
            .OrderByDescending(d => d.Percentage)
            .FirstOrDefaultAsync();
    }
}