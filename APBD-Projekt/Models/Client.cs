﻿using APBD_Projekt.Enums;
using APBD_Projekt.Exceptions;
using APBD_Projekt.RequestModels;

namespace APBD_Projekt.Models;

public abstract class Client
{
    public int IdClient { get; protected set; }
    public string Address { get; protected set; }
    public string Email { get; protected set; }
    public string Phone { get; protected set; }

    public ICollection<Contract> Contracts { get; private set; } = [];
    public ICollection<Subscription> Subscriptions { get; private set; } = [];

    protected Client()
    {
    }

    protected Client(string address, string email, string phone)
    {
        Address = address;
        Email = email;
        Phone = phone;
    }

    public abstract void Delete();

    public virtual bool WasDeleted()
    {
        return false;
    }

    public virtual void Update(UpdateClientRequestModel requestModel)
    {
        Address = requestModel.Address;
        Email = requestModel.Email;
        Phone = requestModel.Phone;
    }

    public abstract void EnsureIsOfType(ClientType clientType);

    public bool IsRegularClient()
    {
        return Subscriptions.Count >= 1 || Contracts.Any(c => c.SignedAt != null);
    }

    public void EnsureHasNoActiveSoftware(Software software)
    {
        if (Subscriptions.Any(sub => sub.IsActiveAndForGivenSoftware(software.IdSoftware)) ||
            Contracts.Any(c => c.IsActiveAndForGivenSoftware(software.IdSoftware)))
        {
            throw new InvalidRequestFormatException(
                $"Client already has active subscription/license for software: {software.Name}");
        }
    }

    public void EnsureIsOwnerOfContract(Contract contract)
    {
        if (contract.IdClient != IdClient)
        {
            throw new InvalidRequestFormatException(
                $"Client: {IdClient} is not an owner of contract: {contract.IdContract}");
        }
    }
}