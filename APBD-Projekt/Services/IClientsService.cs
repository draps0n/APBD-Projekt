using APBD_Projekt.RequestModels;

namespace APBD_Projekt.Services;

public interface IClientsService
{
    Task CreateNewClientAsync(CreateClientRequestModel requestModel);
    Task DeleteClientByIdAsync(int clientId);
}