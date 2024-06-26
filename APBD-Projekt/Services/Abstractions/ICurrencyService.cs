namespace APBD_Projekt.Services.Abstractions;

public interface ICurrencyService
{
    Task<decimal> ConvertFromPlnToCurrencyAsync(decimal money, string currencyCode);
}