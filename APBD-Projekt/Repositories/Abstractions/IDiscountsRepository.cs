using APBD_Projekt.Models;

namespace APBD_Projekt.Repositories.Abstractions;

public interface IDiscountsRepository
{
    Task<Discount?> GetBestActiveDiscountForContract();
}