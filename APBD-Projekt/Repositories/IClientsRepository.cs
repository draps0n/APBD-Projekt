using APBD_Projekt.Models;

namespace APBD_Projekt.Repositories;

public interface IClientsRepository
{
    Task AddIndividualClientAsync(IndividualClient individualClient);
    Task AddCompanyClientAsync(CompanyClient companyClient);
    Task<CompanyClient?> GetClientByKRSAsync(string krs);
    Task<IndividualClient?> GetClientByPESELAsync(string pesel);
    Task<Client?> GetClientByIdAsync(int clientId);
    Task UpdateClientAsync(Client client);
}