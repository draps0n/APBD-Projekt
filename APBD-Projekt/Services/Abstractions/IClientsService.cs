using APBD_Projekt.RequestModels;

namespace APBD_Projekt.Services.Abstractions;

public interface IClientsService
{
    Task CreateNewClientAsync(CreateClientRequestModel requestModel);
    Task DeleteClientByIdAsync(int clientId);

    Task UpdateClientByIdAsync(int clientId, UpdateClientRequestModel requestModel);
}