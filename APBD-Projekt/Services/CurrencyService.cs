using System.Text.Json;
using APBD_Projekt.Exceptions;
using APBD_Projekt.Services.Abstractions;

namespace APBD_Projekt.Services;

public class CurrencyService(HttpClient httpClient) : ICurrencyService
{
    public async Task<decimal> ConvertFromPlnToCurrencyAsync(decimal money, string currencyCode)
    {
        var response = await httpClient.GetAsync("https://open.er-api.com/v6/latest/PLN");
        response.EnsureSuccessStatusCode();

        await using var responseStream = await response.Content.ReadAsStreamAsync();
        using var jsonDocument = await JsonDocument.ParseAsync(responseStream);

        var rates = jsonDocument.RootElement.GetProperty("rates");
        if (rates.TryGetProperty(currencyCode, out var rateElement) && rateElement.TryGetDecimal(out var rate))
        {
            return money * rate;
        }

        throw new BadRequestException($"{currencyCode} is not supported");
    }
}