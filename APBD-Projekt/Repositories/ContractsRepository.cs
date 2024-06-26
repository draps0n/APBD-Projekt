using APBD_Projekt.Models;
using APBD_Projekt.Persistence;
using APBD_Projekt.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace APBD_Projekt.Repositories;

public class ContractsRepository(DatabaseContext context) : IContractsRepository
{
    public async Task AddNewContractAsync(Contract contract)
    {
        await context.Contracts.AddAsync(contract);
        await context.SaveChangesAsync();
    }

    public async Task<Contract?> GetContractByIdAsync(int contractId)
    {
        return await context.Contracts
            .Include(c => c.SoftwareVersion)
            .ThenInclude(sv => sv.Software)
            .Include(c => c.ContractPayments)
            .Include(c => c.Client)
            .Where(c => c.IdContract == contractId)
            .FirstOrDefaultAsync();
    }

    public async Task DeleteContractAsync(Contract contract)
    {
        context.Contracts.Remove(contract);
        await context.SaveChangesAsync();
    }

    public async Task RegisterPaymentAsync(ContractPayment payment)
    {
        await context.ContractPayments.AddAsync(payment);
        await context.SaveChangesAsync();
    }
}