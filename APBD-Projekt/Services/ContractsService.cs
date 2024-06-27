using APBD_Projekt.Exceptions;
using APBD_Projekt.Models;
using APBD_Projekt.Repositories.Abstractions;
using APBD_Projekt.RequestModels;
using APBD_Projekt.ResponseModels;
using APBD_Projekt.Services.Abstractions;

namespace APBD_Projekt.Services;

public class ContractsService(
    IClientsRepository clientsRepository,
    IContractsRepository contractsRepository,
    IDiscountsRepository discountsRepository,
    ISoftwareRepository softwareRepository) : IContractsService
{
    public async Task<CreateContractResponseModel> CreateContractAsync(int clientId,
        CreateContractRequestModel requestModel)
    {
        var client = await GetClientWithBoughtProductsAsync(clientId);

        var startDate = DateTime.Now;
        var discount =
            await discountsRepository.GetBestActiveDiscountForContractAsync(startDate, requestModel.EndDate);

        var softwareVersion = await GetSoftwareVersionByNameAndVersionAsync(requestModel);

        var contract = new Contract(
            startDate,
            requestModel.EndDate,
            requestModel.YearsOfAdditionalSupport,
            client,
            softwareVersion,
            discount);

        await contractsRepository.AddNewContractAsync(contract);
        await contractsRepository.SaveChangesAsync();
        return new CreateContractResponseModel
        {
            ContractId = contract.IdContract,
            ClientId = contract.IdClient,
            FinalPrice = contract.FinalPrice,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            YearsOfSupport = contract.YearsOfSupport,
            SoftwareName = contract.SoftwareVersion.Software.Name,
            SoftwareVersion = contract.SoftwareVersion.Version
        };
    }

    public async Task<CreateContractResponseModel?> PayForContractAsync(int clientId, int contractId, decimal amount)
    {
        var client = await GetClientByIdAsync(clientId);
        var contract = await GetContractByIdAsync(contractId);

        client.EnsureIsOwnerOfContract(contract);
        contract.EnsureIsNotAlreadySigned();

        if (!contract.IsActive())
        {
            var alternativeContract = await CreateAlternativeContract(contract);
            return new CreateContractResponseModel
            {
                ContractId = alternativeContract.IdContract,
                ClientId = alternativeContract.IdClient,
                FinalPrice = alternativeContract.FinalPrice,
                StartDate = alternativeContract.StartDate,
                EndDate = alternativeContract.EndDate,
                YearsOfSupport = alternativeContract.YearsOfSupport,
                SoftwareName = alternativeContract.SoftwareVersion.Software.Name,
                SoftwareVersion = alternativeContract.SoftwareVersion.Version
            };
        }

        var payment = new ContractPayment(amount, contract);
        await contractsRepository.RegisterPaymentAsync(payment);
        await contractsRepository.SaveChangesAsync();
        return null;
    }

    public async Task DeleteContractByIdAsync(int clientId, int contractId)
    {
        var client = await GetClientByIdAsync(clientId);
        var contract = await GetContractByIdAsync(contractId);

        client.EnsureIsOwnerOfContract(contract);

        contractsRepository.DeleteContract(contract);
        await contractsRepository.SaveChangesAsync();
    }

    private async Task<Client> GetClientWithBoughtProductsAsync(int clientId)
    {
        var client = await clientsRepository.GetClientWithBoughtProductsAsync(clientId);
        if (client == null)
        {
            throw new NotFoundException($"Client of id: {clientId} does not exist");
        }

        return client;
    }

    private async Task<SoftwareVersion> GetSoftwareVersionByNameAndVersionAsync(CreateContractRequestModel requestModel)
    {
        var softwareVersion = await softwareRepository.GetSoftwareVersionWithSoftwareByNameAndVersionAsync(
            requestModel.SoftwareName,
            requestModel.SoftwareVersion);

        if (softwareVersion == null)
        {
            throw new NotFoundException(
                $"Software {requestModel.SoftwareName} does not exist or have {requestModel.SoftwareVersion} version");
        }

        return softwareVersion;
    }

    private async Task<Contract> GetContractByIdAsync(int contractId)
    {
        var contract = await contractsRepository.GetContractWithSoftwareClientAndPaymentsByIdAsync(contractId);
        if (contract == null)
        {
            throw new NotFoundException($"Contract of id: {contractId} does not exist");
        }

        return contract;
    }

    private async Task<Client> GetClientByIdAsync(int clientId)
    {
        var client = await clientsRepository.GetClientByIdAsync(clientId);
        if (client == null)
        {
            throw new NotFoundException($"Client of id: {clientId} does not exist");
        }

        return client;
    }

    private async Task<Contract> CreateAlternativeContract(Contract contract)
    {
        var startDate = DateTime.Now;
        var endDate = startDate.AddDays(3);
        var discount =
            await discountsRepository.GetBestActiveDiscountForContractAsync(startDate, endDate);
        var client = await GetClientWithBoughtProductsAsync(contract.IdClient);

        var alternativeContract = new Contract(
            startDate,
            endDate,
            contract.YearsOfSupport - 1,
            client,
            contract.SoftwareVersion,
            discount);

        await contractsRepository.AddNewContractAsync(alternativeContract);
        await contractsRepository.SaveChangesAsync();
        return alternativeContract;
    }
}