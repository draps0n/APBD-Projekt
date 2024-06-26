using APBD_Projekt.RequestModels;
using Contract = APBD_Projekt.Models.Contract;

namespace APBD_Projekt.Services.Abstractions;

public interface IContractsService
{
    Task CreateContractAsync(int clientId, CreateContractRequestModel requestModel);
    Task DeleteContractByIdAsync(int contractId);
}