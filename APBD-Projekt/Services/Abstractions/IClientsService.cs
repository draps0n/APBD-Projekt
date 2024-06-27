using APBD_Projekt.RequestModels;
using APBD_Projekt.ResponseModels;

namespace APBD_Projekt.Services.Abstractions;

public interface IClientsService
{
    Task<CreateClientResponseModel> CreateNewClientAsync(CreateClientRequestModel requestModel);
    Task DeleteClientByIdAsync(int clientId);

    Task<UpdateClientResponseModel> UpdateClientByIdAsync(int clientId, UpdateClientRequestModel requestModel);
}