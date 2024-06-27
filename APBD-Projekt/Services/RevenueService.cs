using APBD_Projekt.Exceptions;
using APBD_Projekt.Repositories.Abstractions;
using APBD_Projekt.ResponseModels;
using APBD_Projekt.Services.Abstractions;

namespace APBD_Projekt.Services;

public class RevenueService(
    IContractsRepository contractsRepository,
    ISubscriptionsRepository subscriptionsRepository,
    ISoftwareRepository softwareRepository,
    ICurrencyService currencyService)
    : IRevenueService
{
    public async Task<GetCurrentRevenueResponseModel> GetCurrentTotalRevenueAsync(string? currencyCode)
    {
        var contractsRevenue = await contractsRepository.GetCurrentContractsRevenueAsync();
        var subscriptionsRevenue = await subscriptionsRepository.GetCurrentSubscriptionsRevenueAsync();
        var totalRevenue = contractsRevenue + subscriptionsRevenue;

        (currencyCode, totalRevenue) = await ConvertToDesiredCurrencyAsync(currencyCode, totalRevenue);

        return new GetCurrentRevenueResponseModel
        {
            CurrentRevenue = totalRevenue,
            Currency = currencyCode
        };
    }

    public async Task<GetCurrentRevenueResponseModel> GetCurrentRevenueForSoftwareAsync(int softwareId,
        string? currencyCode)
    {
        await EnsureSoftwareExistsAsync(softwareId);

        var contractsRevenue = await contractsRepository.GetCurrentContractsRevenueForSoftwareAsync(softwareId);
        var subscriptionsRevenue = await subscriptionsRepository.GetCurrentSubscriptionsRevenueForSoftwareAsync(softwareId);
        var totalRevenue = contractsRevenue + subscriptionsRevenue;

        (currencyCode, totalRevenue) = await ConvertToDesiredCurrencyAsync(currencyCode, totalRevenue);

        return new GetCurrentRevenueResponseModel
        {
            CurrentRevenue = totalRevenue,
            Currency = currencyCode
        };
    }

    public async Task<GetForecastedRevenueResponse> GetForecastedTotalRevenueAsync(string? currencyCode)
    {
        var contractsRevenue = await contractsRepository.GetForecastedContractsRevenueAsync();
        var currentSubscriptionsRevenue = await subscriptionsRepository.GetCurrentSubscriptionsRevenueAsync();
        var notYetPaidSubscriptionsRevenue = await subscriptionsRepository.GetNotYetPaidSubscriptionsRevenueAsync();
        var totalRevenue = contractsRevenue + currentSubscriptionsRevenue + notYetPaidSubscriptionsRevenue;

        (currencyCode, totalRevenue) = await ConvertToDesiredCurrencyAsync(currencyCode, totalRevenue);

        return new GetForecastedRevenueResponse
        {
            ForecastedRevenue = totalRevenue,
            Currency = currencyCode
        };
    }

    public async Task<GetForecastedRevenueResponse> GetForecastedRevenueForSoftwareAsync(int softwareId,
        string? currencyCode)
    {
        await EnsureSoftwareExistsAsync(softwareId);

        var contractsRevenue = await contractsRepository.GetForecastedContractsRevenueForSoftwareAsync(softwareId);
        var currentSubscriptionsRevenue =
            await subscriptionsRepository.GetCurrentSubscriptionsRevenueForSoftwareAsync(softwareId);
        var notYetPaidSubscriptionsRevenue =
            await subscriptionsRepository.GetNotYetPaidSubscriptionsRevenueForSoftwareAsync(softwareId);
        var totalRevenue = contractsRevenue + currentSubscriptionsRevenue + notYetPaidSubscriptionsRevenue;

        (currencyCode, totalRevenue) = await ConvertToDesiredCurrencyAsync(currencyCode, totalRevenue);

        return new GetForecastedRevenueResponse
        {
            ForecastedRevenue = totalRevenue,
            Currency = currencyCode
        };
    }

    private async Task<(string currencyCode, decimal totalRevenue)> ConvertToDesiredCurrencyAsync(string? currencyCode,
        decimal totalRevenue)
    {
        if (currencyCode == null)
        {
            currencyCode = "PLN";
        }

        if (currencyCode != "PLN")
        {
            totalRevenue = await currencyService.ConvertFromPlnToCurrencyAsync(totalRevenue, currencyCode);
        }

        return (currencyCode, totalRevenue);
    }

    private async Task EnsureSoftwareExistsAsync(int softwareId)
    {
        var software = await softwareRepository.GetSoftwareByIdAsync(softwareId);

        if (software == null)
        {
            throw new NotFoundException($"Software of id: {softwareId} does not exist");
        }
    }
}