using APBD_Projekt.Models;

namespace APBD_Projekt.Repositories.Abstractions;

public interface IContractsRepository
{
    Task AddNewContractAsync(Contract contract);
    Task<Contract?> GetContractByIdAsync(int contractId);
    Task DeleteContractAsync(Contract contract);
    Task RegisterPaymentAsync(ContractPayment payment);
    Task<decimal> GetCurrentContractsRevenueAsync();
    Task<decimal> GetCurrentContractsRevenueForSoftware(int softwareId);
    Task<decimal> GetForecastedContractsRevenueAsync();
    Task<decimal> GetForecastedContractsRevenueForSoftwareAsync(int softwareId);
}