using APBD_Projekt.Models;

namespace APBD_Projekt.Repositories.Abstractions;

public interface IContractsRepository
{
    Task AddNewContractAsync(Contract contract);
    Task<Contract?> GetContractWithSoftwareClientAndPaymentsByIdAsync(int contractId);
    void DeleteContract(Contract contract);
    Task RegisterPaymentAsync(ContractPayment payment);
    Task<decimal> GetCurrentContractsRevenueAsync();
    Task<decimal> GetCurrentContractsRevenueForSoftwareAsync(int softwareId);
    Task<decimal> GetForecastedContractsRevenueAsync();
    Task<decimal> GetForecastedContractsRevenueForSoftwareAsync(int softwareId);
    Task SaveChangesAsync();
}