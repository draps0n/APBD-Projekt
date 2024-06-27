using APBD_Projekt.Models;
using APBD_Projekt.Persistence;
using APBD_Projekt.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace APBD_Projekt.Repositories;

public class ClientsRepository(DatabaseContext context) : IClientsRepository
{
    public async Task AddIndividualClientAsync(IndividualClient individualClient)
    {
        await context.IndividualClients.AddAsync(individualClient);
    }

    public async Task AddCompanyClientAsync(CompanyClient companyClient)
    {
        await context.CompanyClients.AddAsync(companyClient);
    }

    public async Task<CompanyClient?> GetClientByKrsAsync(string krs)
    {
        return await context.CompanyClients
            .Where(c => c.KRS == krs)
            .FirstOrDefaultAsync();
    }

    public async Task<IndividualClient?> GetClientByPeselAsync(string pesel)
    {
        return await context.IndividualClients
            .Where(c => c.PESEL == pesel)
            .FirstOrDefaultAsync();
    }

    public async Task<Client?> GetClientByIdAsync(int clientId)
    {
        return await context.Clients
            .Where(c => c.IdClient == clientId)
            .FirstOrDefaultAsync();
    }

    public void UpdateClient(Client client)
    {
        context.Clients.Update(client);
    }

    public async Task<Client?> GetClientWithBoughtProductsAsync(int clientId)
    {
        return await context.Clients
            .Include(cl => cl.Subscriptions)
            .ThenInclude(sub => sub.SubscriptionOffer)
            .ThenInclude(subOff => subOff.Software)
            .Include(cl => cl.Contracts)
            .ThenInclude(c => c.SoftwareVersion)
            .ThenInclude(sv => sv.Software)
            .Where(cl => cl.IdClient == clientId)
            .FirstOrDefaultAsync();
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}