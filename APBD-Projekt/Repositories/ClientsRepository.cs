using APBD_Projekt.Context;
using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_Projekt.Repositories;

public class ClientsRepository(DatabaseContext context) : IClientsRepository
{
    public async Task AddIndividualClientAsync(IndividualClient individualClient)
    {
        await context.IndividualClients.AddAsync(individualClient);
        await context.SaveChangesAsync();
    }

    public async Task AddCompanyClientAsync(CompanyClient companyClient)
    {
        await context.CompanyClients.AddAsync(companyClient);
        await context.SaveChangesAsync();
    }

    public async Task<CompanyClient?> GetClientByKRSAsync(string krs)
    {
        return await context.CompanyClients.Where(c => c.KRS == krs).FirstOrDefaultAsync();
    }

    public async Task<IndividualClient?> GetClientByPESELAsync(string pesel)
    {
        return await context.IndividualClients.Where(c => c.PESEL == pesel).FirstOrDefaultAsync();
    }
}