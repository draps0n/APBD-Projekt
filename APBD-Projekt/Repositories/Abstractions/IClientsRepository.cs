using APBD_Projekt.Models;

namespace APBD_Projekt.Repositories.Abstractions;

public interface IClientsRepository
{
    Task AddIndividualClientAsync(IndividualClient individualClient);
    Task AddCompanyClientAsync(CompanyClient companyClient);
    Task<CompanyClient?> GetClientByKrsAsync(string krs);
    Task<IndividualClient?> GetClientByPeselAsync(string pesel);
    Task<Client?> GetClientByIdAsync(int clientId);
    void UpdateClient(Client client);
    Task<Client?> GetClientWithBoughtProductsAsync(int clientId);
    Task SaveChangesAsync();
}