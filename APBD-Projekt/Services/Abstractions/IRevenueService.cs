using APBD_Projekt.ResponseModels;

namespace APBD_Projekt.Services.Abstractions;

public interface IRevenueService
{
    Task<GetCurrentRevenueResponseModel> GetCurrentTotalRevenueAsync(string? currencyCode);
    Task<GetCurrentRevenueResponseModel> GetCurrentRevenueForSoftwareAsync(int softwareId, string? currencyCode);
    Task<GetForecastedRevenueResponse> GetForecastedTotalRevenueAsync(string? currencyCode);
    Task<GetForecastedRevenueResponse> GetForecastedRevenueForSoftwareAsync(int softwareId, string? currencyCode);
}