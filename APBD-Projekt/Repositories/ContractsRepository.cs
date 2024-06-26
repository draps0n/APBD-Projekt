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

    public Task DeleteContractByIdAsync(int contractId)
    {
        throw new NotImplementedException();
    }
}