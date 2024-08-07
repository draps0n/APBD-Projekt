﻿using APBD_Projekt.Models;
using APBD_Projekt.Persistence;
using APBD_Projekt.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace APBD_Projekt.Repositories;

public class ContractsRepository(DatabaseContext context) : IContractsRepository
{
    public async Task AddNewContractAsync(Contract contract)
    {
        await context.Contracts.AddAsync(contract);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task<Contract?> GetContractWithSoftwareClientAndPaymentsByIdAsync(int contractId)
    {
        return await context.Contracts
            .Include(c => c.SoftwareVersion)
            .ThenInclude(sv => sv.Software)
            .Include(c => c.ContractPayments)
            .Include(c => c.Client)
            .Where(c => c.IdContract == contractId)
            .FirstOrDefaultAsync();
    }

    public void DeleteContract(Contract contract)
    {
        context.Contracts.Remove(contract);
    }

    public async Task RegisterPaymentAsync(ContractPayment payment)
    {
        await context.ContractPayments.AddAsync(payment);
    }

    public async Task<decimal> GetCurrentContractsRevenueAsync()
    {
        return await context.Contracts
            .Where(c => c.SignedAt != null)
            .SumAsync(c => c.FinalPrice);
    }

    public async Task<decimal> GetCurrentContractsRevenueForSoftwareAsync(int softwareId)
    {
        return await context.Contracts
            .Where(c => c.SignedAt != null && c.SoftwareVersion.IdSoftware == softwareId)
            .SumAsync(c => c.FinalPrice);
    }

    public async Task<decimal> GetForecastedContractsRevenueAsync()
    {
        return await context.Contracts
            .Where(c => c.SignedAt != null || DateTime.Now < c.EndDate)
            .SumAsync(c => c.FinalPrice);
    }

    public async Task<decimal> GetForecastedContractsRevenueForSoftwareAsync(int softwareId)
    {
        return await context.Contracts
            .Where(c => (c.SignedAt != null || DateTime.Now < c.EndDate) && c.SoftwareVersion.IdSoftware == softwareId)
            .SumAsync(c => c.FinalPrice);
    }
}