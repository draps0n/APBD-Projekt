using APBD_Projekt.Models;
using APBD_Projekt.Repositories.Abstractions;

namespace APBD_Projekt.Tests.TestObjects;

public class FakeContractsRepository(List<Contract> contracts, List<ContractPayment> contractPayments)
    : IContractsRepository
{
    public async Task AddNewContractAsync(Contract contract)
    {
        contracts.Add(contract);
    }

    public async Task<Contract?> GetContractWithSoftwareClientAndPaymentsByIdAsync(int contractId)
    {
        return contracts
            .FirstOrDefault(c => c.IdContract == contractId);
    }

    public void DeleteContract(Contract contract)
    {
        contracts.Remove(contract);
    }

    public async Task RegisterPaymentAsync(ContractPayment payment)
    {
        contractPayments.Add(payment);
    }

    public async Task<decimal> GetCurrentContractsRevenueAsync()
    {
        return contracts
            .Where(c => c.SignedAt != null)
            .Sum(c => c.FinalPrice);
    }

    public async Task<decimal> GetCurrentContractsRevenueForSoftwareAsync(int softwareId)
    {
        return contracts
            .Where(c => c.SignedAt != null && c.SoftwareVersion.IdSoftware == softwareId)
            .Sum(c => c.FinalPrice);
    }

    public async Task<decimal> GetForecastedContractsRevenueAsync()
    {
        return contracts
            .Where(c => c.SignedAt != null || DateTime.Now < c.EndDate)
            .Sum(c => c.FinalPrice);
    }

    public async Task<decimal> GetForecastedContractsRevenueForSoftwareAsync(int softwareId)
    {
        return contracts
            .Where(c => (c.SignedAt != null || DateTime.Now < c.EndDate) && c.SoftwareVersion.IdSoftware == softwareId)
            .Sum(c => c.FinalPrice);
    }

    public async Task SaveChangesAsync()
    {
    }
}