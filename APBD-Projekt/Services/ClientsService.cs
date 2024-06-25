using APBD_Projekt.Enums;
using APBD_Projekt.Exceptions;
using APBD_Projekt.Models;
using APBD_Projekt.Repositories;
using APBD_Projekt.RequestModels;

namespace APBD_Projekt.Services;

public class ClientsService(IClientsRepository clientsRepository) : IClientsService
{
    public async Task CreateNewClientAsync(CreateClientRequestModel requestModel)
    {
        EnsureRequestModelIsValid(requestModel);

        if (requestModel.ClientType == ClientType.Individual)
        {
            await EnsurePESELIsUnique(requestModel.PESEL!);
            var individualClient = new IndividualClient(
                requestModel.Address,
                requestModel.Email,
                requestModel.Phone,
                requestModel.Name!,
                requestModel.LastName!,
                requestModel.PESEL!
            );

            await clientsRepository.AddIndividualClientAsync(individualClient);
        }
        else if (requestModel.ClientType == ClientType.Company)
        {
            await EnsureKRSIsUnique(requestModel.KRS!);

            var companyClient = new CompanyClient(
                requestModel.Address,
                requestModel.Email,
                requestModel.Phone,
                requestModel.CompanyName!,
                requestModel.KRS!
            );

            await clientsRepository.AddCompanyClientAsync(companyClient);
        }
    }

    private async Task EnsureKRSIsUnique(string krs)
    {
        var client = await clientsRepository.GetClientByKRSAsync(krs);
        if (client != null)
        {
            throw new BadRequestException($"Client with KRS {krs} already exists");
        }
    }

    private async Task EnsurePESELIsUnique(string pesel)
    {
        var client = await clientsRepository.GetClientByPESELAsync(pesel);
        if (client != null)
        {
            throw new BadRequestException($"Client with PESEL {pesel} already exists");
        }
    }

    private static void EnsureRequestModelIsValid(CreateClientRequestModel requestModel)
    {
        if (!DoesMatchIndividualClient(requestModel) && !DoesMatchCompanyClient(requestModel))
        {
            throw new BadRequestException("Request model is invalid");
        }
    }

    private static bool DoesMatchIndividualClient(CreateClientRequestModel requestModel)
    {
        return requestModel.ClientType == ClientType.Individual && requestModel.Name != null && requestModel.LastName != null && requestModel.PESEL != null;
    }

    private static bool DoesMatchCompanyClient(CreateClientRequestModel requestModel)
    {
        return requestModel.ClientType == ClientType.Company && requestModel.CompanyName != null && requestModel.KRS != null;
    }
}