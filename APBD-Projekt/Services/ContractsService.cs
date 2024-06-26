using APBD_Projekt.Exceptions;
using APBD_Projekt.Models;
using APBD_Projekt.Repositories.Abstractions;
using APBD_Projekt.RequestModels;
using APBD_Projekt.Services.Abstractions;

namespace APBD_Projekt.Services;

public class ContractsService(
    IClientsRepository clientsRepository,
    IContractsRepository contractsRepository,
    IDiscountsRepository discountsRepository,
    ISoftwareRepository softwareRepository) : IContractsService
{
    public async Task CreateContractAsync(int clientId, CreateContractRequestModel requestModel)
    {
        var client = await clientsRepository.GetClientWithBoughtProductsAsync(clientId);
        if (client == null)
        {
            throw new NotFoundException($"Client of id: {clientId} does not exist");
        }

        var discount = await discountsRepository.GetBestActiveDiscountForContract();

        var softwareVersion = await softwareRepository.GetSoftwareVersionByNameAndVersion(requestModel.SoftwareName,
            requestModel.SoftwareVersion);

        if (softwareVersion == null)
        {
            throw new NotFoundException(
                $"Software {requestModel.SoftwareName} does not exist or have {requestModel.SoftwareVersion} version");
        }

        var contract = new Contract(
            requestModel.StartDate,
            requestModel.EndDate,
            requestModel.YearsOfAdditionalSupport,
            client,
            softwareVersion,
            discount);

        await contractsRepository.AddNewContractAsync(contract);
    }

    public async Task DeleteContractByIdAsync(int contractId)
    {
        throw new NotImplementedException();
    }
}