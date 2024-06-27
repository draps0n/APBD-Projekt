using System.ComponentModel.DataAnnotations;
using APBD_Projekt.Enums;
using APBD_Projekt.Exceptions;
using APBD_Projekt.RequestModels;

namespace APBD_Projekt.Models;

public class IndividualClient : Client
{
    public string Name { get; private set; }

    public string LastName { get; private set; }

    public string PESEL { get; private set; }

    public bool IsDeleted { get; private set; }

    protected IndividualClient()
    {
    }

    public IndividualClient(string address, string email, string phone, string name, string lastName, string pesel) :
        base(address, email, phone)
    {
        Name = name;
        LastName = lastName;
        PESEL = pesel;
        IsDeleted = false;
    }

    public override void Delete()
    {
        Address = string.Empty;
        Email = string.Empty;
        Phone = string.Empty;
        Name = string.Empty;
        LastName = string.Empty;
        PESEL = string.Empty;
        IsDeleted = true;
    }

    public override bool WasDeleted()
    {
        return IsDeleted;
    }

    public override void Update(UpdateClientRequestModel requestModel)
    {
        EnsureClientNotDeleted();
        base.Update(requestModel);
        Name = requestModel.Name!;
        LastName = requestModel.LastName!;
    }

    private void EnsureClientNotDeleted()
    {
        if (IsDeleted)
        {
            throw new BadRequestException("Cannot update deleted client");
        }
    }

    public override void EnsureIsOfType(ClientType clientType)
    {
        if (clientType != ClientType.Individual)
        {
            throw new BadRequestException($"Client of id: {IdClient} is a individual client!");
        }
    }
}