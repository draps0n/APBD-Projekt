using APBD_Projekt.Enums;
using APBD_Projekt.Exceptions;
using APBD_Projekt.Models;
using APBD_Projekt.Repositories.Abstractions;
using APBD_Projekt.RequestModels;
using APBD_Projekt.Services.Abstractions;

namespace APBD_Projekt.Services;

public class ClientsService(IClientsRepository clientsRepository) : IClientsService
{
    public async Task CreateNewClientAsync(CreateClientRequestModel requestModel)
    {
        var clientType = GetClientTypeFromString(requestModel.ClientType);
        EnsureCreateRequestModelIsValid(requestModel, clientType);

        switch (clientType)
        {
            case ClientType.Individual:
            {
                await EnsurePeselIsUnique(requestModel.PESEL!);
                var individualClient = new IndividualClient(
                    requestModel.Address,
                    requestModel.Email,
                    requestModel.Phone,
                    requestModel.Name!,
                    requestModel.LastName!,
                    requestModel.PESEL!
                );

                await clientsRepository.AddIndividualClientAsync(individualClient);
                break;
            }
            case ClientType.Company:
            {
                await EnsureKrsIsUnique(requestModel.KRS!);

                var companyClient = new CompanyClient(
                    requestModel.Address,
                    requestModel.Email,
                    requestModel.Phone,
                    requestModel.CompanyName!,
                    requestModel.KRS!
                );

                await clientsRepository.AddCompanyClientAsync(companyClient);
                break;
            }
        }
    }
    
    public async Task DeleteClientByIdAsync(int clientId)
    {
        var client = await clientsRepository.GetClientByIdAsync(clientId);
        if (client == null)
        {
            throw new NotFoundException($"Client with id: {clientId} does not exist");
        }

        client.Delete();
        await clientsRepository.UpdateClientAsync(client);
    }

    public async Task UpdateClientByIdAsync(int clientId, UpdateClientRequestModel requestModel)
    {
        var clientType = GetClientTypeFromString(requestModel.ClientType);
        EnsureUpdateRequestModelIsValid(requestModel, clientType);

        var client = await clientsRepository.GetClientByIdAsync(clientId);
        if (client == null)
        {
            throw new NotFoundException($"Client with id: {clientId} does not exist");
        }

        client.EnsureIsOfType(clientType);

        client.Update(requestModel);
        await clientsRepository.UpdateClientAsync(client);
    }
    
    private static ClientType GetClientTypeFromString(string clientTypeString)
    {
        if (!Enum.TryParse(clientTypeString, out ClientType clientType))
        {
            throw new BadRequestException(
                $"Client type must be {ClientType.Company.ToString()} or {ClientType.Individual.ToString()}");
        }

        return clientType;
    }

    private async Task EnsureKrsIsUnique(string krs)
    {
        var client = await clientsRepository.GetClientByKrsAsync(krs);
        if (client != null)
        {
            throw new BadRequestException($"Client with KRS {krs} already exists");
        }
    }

    private async Task EnsurePeselIsUnique(string pesel)
    {
        var client = await clientsRepository.GetClientByPeselAsync(pesel);
        if (client != null)
        {
            throw new BadRequestException($"Client with PESEL {pesel} already exists");
        }
    }

    private static void EnsureCreateRequestModelIsValid(CreateClientRequestModel model, ClientType clientType)
    {
        if (!DoesMatchIndividualClient(model.Name, model.LastName, model.PESEL, clientType) &&
            !DoesMatchCompanyClient(model.CompanyName, model.KRS, clientType))
        {
            throw new BadRequestException("Request model is invalid");
        }
    }

    private static bool DoesMatchIndividualClient(string? name, string? lastName, string? pesel, ClientType clientType)
    {
        return clientType == ClientType.Individual && name != null && lastName != null && pesel != null;
    }

    private static bool DoesMatchCompanyClient(string? companyName, string? krs, ClientType clientType)
    {
        return clientType == ClientType.Company && companyName != null && krs != null;
    }

    private static bool DoesMatchIndividualClient(string? name, string? lastName, ClientType clientType)
    {
        return clientType == ClientType.Individual && name != null && lastName != null;
    }

    private static bool DoesMatchCompanyClient(string? companyName, ClientType clientType)
    {
        return clientType == ClientType.Company && companyName != null;
    }

    private static void EnsureUpdateRequestModelIsValid(UpdateClientRequestModel model, ClientType clientType)
    {
        if (!DoesMatchIndividualClient(model.Name, model.LastName, clientType) &&
            !DoesMatchCompanyClient(model.CompanyName, clientType))
        {
            throw new BadRequestException("Request model is invalid");
        }
    }
}