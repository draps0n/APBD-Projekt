using APBD_Projekt.RequestModels;
using APBD_Projekt.ResponseModels;

namespace APBD_Projekt.Services.Abstractions;

public interface IContractsService
{
    Task<CreateContractResponseModel> CreateContractAsync(int clientId, CreateContractRequestModel requestModel);
    Task DeleteContractByIdAsync(int clientId, int contractId);
    Task<(bool, AlternativeContractResponseModel?)> PayForContractAsync(int clientId, int contractId, decimal amount);
}