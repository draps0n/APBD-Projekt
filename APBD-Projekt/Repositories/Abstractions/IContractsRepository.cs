using APBD_Projekt.Models;

namespace APBD_Projekt.Repositories.Abstractions;

public interface IContractsRepository
{
    Task AddNewContractAsync(Contract contract);
    Task<Contract?> GetContractByIdAsync(int contractId);
    Task DeleteContractAsync(Contract contract);
    Task RegisterPaymentAsync(ContractPayment payment);
}