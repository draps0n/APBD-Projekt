﻿using APBD_Projekt.Enums;
using APBD_Projekt.Exceptions;
using APBD_Projekt.Models;
using APBD_Projekt.Repositories.Abstractions;
using APBD_Projekt.RequestModels;
using APBD_Projekt.ResponseModels;
using APBD_Projekt.Services.Abstractions;

namespace APBD_Projekt.Services;

public class ClientsService(IClientsRepository clientsRepository) : IClientsService
{
    public async Task<CreateClientResponseModel> CreateNewClientAsync(CreateClientRequestModel requestModel)
    {
        var clientType = GetClientTypeFromString(requestModel.ClientType);
        EnsureCreateClientRequestModelIsValid(requestModel, clientType);

        return clientType switch
        {
            ClientType.Individual => await CreateIndividualClientAsync(requestModel),
            ClientType.Company => await CreateCompanyClientAsync(requestModel),
            _ => throw new BadRequestException($"Client type {clientType.ToString()} is not supported")
        };
    }

    public async Task DeleteClientByIdAsync(int clientId)
    {
        var client = await GetClientByIdAsync(clientId);

        client.Delete();
        await clientsRepository.UpdateClientAsync(client); // TODO : uof
    }

    public async Task<UpdateClientResponseModel> UpdateClientByIdAsync(int clientId,
        UpdateClientRequestModel requestModel)
    {
        var clientType = GetClientTypeFromString(requestModel.ClientType);
        EnsureUpdateRequestModelIsValid(requestModel, clientType);

        var client = await GetClientByIdAsync(clientId);

        client.EnsureIsOfType(clientType);

        client.Update(requestModel);
        await clientsRepository.UpdateClientAsync(client); // TODO : uof

        return new UpdateClientResponseModel
        {
            IdClient = client.IdClient,
            Address = client.Address,
            ClientType = clientType.ToString(),
            Email = client.Email,
            Phone = client.Phone,
            Name = clientType == ClientType.Individual ? ((IndividualClient)client).Name : null,
            LastName = clientType == ClientType.Individual ? ((IndividualClient)client).LastName : null,
            PESEL = clientType == ClientType.Individual ? ((IndividualClient)client).PESEL : null,
            CompanyName = clientType == ClientType.Company ? ((CompanyClient)client).CompanyName : null,
            KRS = clientType == ClientType.Company ? ((CompanyClient)client).KRS : null
        };
    }

    private async Task<CreateClientResponseModel> CreateCompanyClientAsync(CreateClientRequestModel requestModel)
    {
        await EnsureKrsIsUniqueAsync(requestModel.KRS!);

        var companyClient = new CompanyClient(
            requestModel.Address,
            requestModel.Email,
            requestModel.Phone,
            requestModel.CompanyName!,
            requestModel.KRS!
        );

        await clientsRepository.AddCompanyClientAsync(companyClient);
        return new CreateClientResponseModel
        {
            IdClient = companyClient.IdClient,
            Address = companyClient.Address,
            ClientType = requestModel.ClientType,
            Email = companyClient.Email,
            Phone = companyClient.Phone,
            CompanyName = companyClient.CompanyName,
            KRS = companyClient.KRS
        };
    }

    private async Task<CreateClientResponseModel> CreateIndividualClientAsync(CreateClientRequestModel requestModel)
    {
        await EnsurePeselIsUniqueAsync(requestModel.PESEL!);
        var individualClient = new IndividualClient(
            requestModel.Address,
            requestModel.Email,
            requestModel.Phone,
            requestModel.Name!,
            requestModel.LastName!,
            requestModel.PESEL!
        );

        await clientsRepository.AddIndividualClientAsync(individualClient);
        return new CreateClientResponseModel
        {
            IdClient = individualClient.IdClient,
            Address = individualClient.Address,
            ClientType = requestModel.ClientType,
            Email = individualClient.Email,
            Phone = individualClient.Phone,
            Name = individualClient.Name,
            LastName = individualClient.LastName,
            PESEL = individualClient.PESEL
        };
    }

    private async Task<Client> GetClientByIdAsync(int clientId)
    {
        var client = await clientsRepository.GetClientByIdAsync(clientId);
        if (client == null)
        {
            throw new NotFoundException($"Client with id: {clientId} does not exist");
        }

        return client;
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

    private async Task EnsureKrsIsUniqueAsync(string krs)
    {
        var client = await clientsRepository.GetClientByKrsAsync(krs);
        if (client != null)
        {
            throw new BadRequestException($"Client with KRS {krs} already exists");
        }
    }

    private async Task EnsurePeselIsUniqueAsync(string pesel)
    {
        var client = await clientsRepository.GetClientByPeselAsync(pesel);
        if (client != null)
        {
            throw new BadRequestException($"Client with PESEL {pesel} already exists");
        }
    }

    private static void EnsureCreateClientRequestModelIsValid(CreateClientRequestModel model, ClientType clientType)
    {
        if (!DoesMatchIndividualClient(model.Name, model.LastName, model.PESEL, clientType) &&
            !DoesMatchCompanyClient(model.CompanyName, model.KRS, clientType))
        {
            throw new BadRequestException($"Request model does not match given client type: {clientType}");
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
            throw new BadRequestException($"Request model does not match given client type: {clientType}");
        }
    }
}