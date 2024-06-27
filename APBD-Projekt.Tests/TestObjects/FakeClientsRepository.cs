using APBD_Projekt.Models;
using APBD_Projekt.Repositories.Abstractions;

namespace APBD_Projekt.Tests.TestObjects;

public class FakeClientsRepository : IClientsRepository
{
    private readonly List<Client> _clients;
    private readonly List<IndividualClient> _individualClients;
    private readonly List<CompanyClient> _companyClients;

    public FakeClientsRepository(List<IndividualClient> individualClients, List<CompanyClient> companyClients)
    {
        _individualClients = individualClients;
        _companyClients = companyClients;
        _clients = [];

        _clients.AddRange(individualClients);
        _clients.AddRange(companyClients);
    }

    public async Task AddIndividualClientAsync(IndividualClient individualClient)
    {
        _individualClients.Add(individualClient);
        _clients.Add(individualClient);
    }

    public async Task AddCompanyClientAsync(CompanyClient companyClient)
    {
        _companyClients.Add(companyClient);
        _clients.Add(companyClient);
    }

    public async Task<CompanyClient?> GetClientByKrsAsync(string krs)
    {
        return _companyClients
            .FirstOrDefault(c => c.KRS == krs);
    }

    public async Task<IndividualClient?> GetClientByPeselAsync(string pesel)
    {
        return _individualClients
            .FirstOrDefault(c => c.PESEL == pesel);
    }

    public async Task<Client?> GetClientByIdAsync(int clientId)
    {
        return _clients
            .FirstOrDefault(c => c.IdClient == clientId);
    }

    public void UpdateClient(Client client)
    {
        var clientInDb = _clients.FirstOrDefault(cl => cl.IdClient == client.IdClient)!;
        _clients.Remove(clientInDb);
        _clients.Add(client);
        if (client is IndividualClient individualClient)
        {
            _individualClients.Remove((IndividualClient)clientInDb);
            _individualClients.Add(individualClient);
        }
        else
        {
            _companyClients.Remove((CompanyClient)clientInDb);
            _companyClients.Add((CompanyClient)client);
        }
    }

    public async Task<Client?> GetClientWithBoughtProductsAsync(int clientId)
    {
        return _clients
            .FirstOrDefault(cl => cl.IdClient == clientId);
    }

    public async Task SaveChangesAsync()
    {
    }
}