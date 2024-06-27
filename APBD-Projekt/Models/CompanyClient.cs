using APBD_Projekt.Enums;
using APBD_Projekt.Exceptions;
using APBD_Projekt.RequestModels;

namespace APBD_Projekt.Models;

public class CompanyClient : Client
{
    public string CompanyName { get; private set; }
    public string KRS { get; private set; }

    protected CompanyClient()
    {
    }

    public CompanyClient(string address, string email, string phone, string companyName, string krs) : base(address,
        email, phone)
    {
        CompanyName = companyName;
        KRS = krs;
    }

    public override void Delete()
    {
        throw new ClientTypeException("Company clients cannot be deleted");
    }

    public override void Update(UpdateClientRequestModel requestModel)
    {
        base.Update(requestModel);
        CompanyName = requestModel.CompanyName!;
    }

    public override void EnsureIsOfType(ClientType clientType)
    {
        if (clientType != ClientType.Company)
        {
            throw new ClientTypeException($"Client of id: {IdClient} is a company client!");
        }
    }
}