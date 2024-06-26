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
        var client = await clientsRepository.GetClientWithBoughtProductsAsync(clientId);
        if (client == null)
        {
            throw new NotFoundException($"Client of id: {clientId} does not exist");
        }

        var startDate = DateTime.Now;
        var discount =
            await discountsRepository.GetBestActiveDiscountForContract(startDate, requestModel.EndDate);

        var softwareVersion = await softwareRepository.GetSoftwareVersionByNameAndVersion(requestModel.SoftwareName,
            requestModel.SoftwareVersion);

        if (softwareVersion == null)
        {
            throw new NotFoundException(
                $"Software {requestModel.SoftwareName} does not exist or have {requestModel.SoftwareVersion} version");
        }

        var contract = new Contract(
            startDate,
            requestModel.EndDate,
            requestModel.YearsOfAdditionalSupport,
            client,
            softwareVersion,
            discount);

        await contractsRepository.AddNewContractAsync(contract);
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

    public async Task DeleteContractByIdAsync(int clientId, int contractId)
    {
        var client = await clientsRepository.GetClientByIdAsync(clientId);
        if (client == null)
        {
            throw new NotFoundException($"Client of id: {clientId} does not exist");
        }

        var contract = await contractsRepository.GetContractByIdAsync(contractId);
        if (contract == null)
        {
            throw new NotFoundException($"Contract of id: {contractId} does not exist");
        }

        EnsureClientIsOwnerOfContract(client, contract);

        await contractsRepository.DeleteContractAsync(contract);
    }

    public async Task<CreateContractResponseModel?> PayForContractAsync(int clientId, int contractId, decimal amount)
    {
        var client = await clientsRepository.GetClientByIdAsync(clientId);
        if (client == null)
        {
            throw new NotFoundException($"Client of id: {clientId} does not exist");
        }

        var contract = await contractsRepository.GetContractByIdAsync(contractId);
        if (contract == null)
        {
            throw new NotFoundException($"Contract of id: {contractId} does not exist");
        }

        EnsureClientIsOwnerOfContract(client, contract);
        EnsureContractIsNotAlreadySigned(contract);
        if (!CheckIfContractIsValid(contract))
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
        return null;
    }

    private static bool CheckIfContractIsValid(Contract contract)
    {
        return contract.EndDate > DateTime.Now;
    }

    private async Task<Contract> CreateAlternativeContract(Contract contract)
    {
        var startDate = DateTime.Now;
        var endDate = startDate.AddDays(3);
        var discount =
            await discountsRepository.GetBestActiveDiscountForContract(startDate, endDate);
        var client = await clientsRepository.GetClientWithBoughtProductsAsync(contract.IdClient);

        var alternativeContract = new Contract(
            startDate,
            endDate,
            contract.YearsOfSupport - 1,
            client!,
            contract.SoftwareVersion,
            discount);

        await contractsRepository.AddNewContractAsync(alternativeContract);
        return alternativeContract;
    }

    private static void EnsureContractIsNotAlreadySigned(Contract contract)
    {
        if (contract.SignedAt != null)
        {
            throw new BadRequestException("Cannot pay for contract already signed");
        }
    }

    private static void EnsureClientIsOwnerOfContract(Client client, Contract contract)
    {
        if (contract.IdClient != client.IdClient)
        {
            throw new BadRequestException(
                $"Client: {client.IdClient} is not an owner of contract: {contract.IdContract}");
        }
    }
}