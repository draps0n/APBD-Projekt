﻿using APBD_Projekt.Exceptions;
using APBD_Projekt.Models;
using APBD_Projekt.Repositories.Abstractions;
using APBD_Projekt.RequestModels;
using APBD_Projekt.ResponseModels;
using APBD_Projekt.Services.Abstractions;

namespace APBD_Projekt.Services;

public class SubscriptionsService(
    IClientsRepository clientsRepository,
    ISubscriptionsRepository subscriptionsRepository,
    ISoftwareRepository softwareRepository,
    IDiscountsRepository discountsRepository) : ISubscriptionsService
{
    public async Task<CreateSubscriptionResponseModel> CreateSubscriptionAsync(int clientId,
        CreateSubscriptionRequestModel requestModel)
    {
        var client = await clientsRepository.GetClientWithBoughtProductsAsync(clientId);
        if (client == null)
        {
            throw new NotFoundException($"Client of id: {clientId} does not exist");
        }

        var subscriptionOffer =
            await softwareRepository.GetSoftwareSubscriptionOfferByNameAsync(requestModel.SoftwareName,
                requestModel.SubscriptionOfferName);
        if (subscriptionOffer == null)
        {
            throw new NotFoundException(
                $"Subscription offer of name: {requestModel.SubscriptionOfferName} or software of name: {requestModel.SoftwareName} does not exist");
        }

        var startDate = DateTime.Now;
        var discount = await discountsRepository.GetBestActiveDiscountForSubscriptionAsync(startDate);

        var subscription = new Subscription(startDate, client, subscriptionOffer, discount);

        await subscriptionsRepository.CreateSubscriptionAsync(subscription);

        return new CreateSubscriptionResponseModel
        {
            IdSubscription = subscription.IdSubscription,
            MonthsPerRenewalTime = subscription.SubscriptionOffer.MonthsPerRenewalTime,
            SoftwareName = requestModel.SoftwareName,
            SubscriptionOfferName = requestModel.SubscriptionOfferName,
            Fee = subscription.CalculateFee()
        };
    }

    public async Task PayForSubscriptionAsync(int clientId, int subscriptionId, PayForSubscriptionRequestModel requestModel)
    {
        var client = await clientsRepository.GetClientByIdAsync(clientId);
        if (client == null)
        {
            throw new NotFoundException($"Client of id: {clientId} does not exist");
        }

        var subscription = await subscriptionsRepository.GetSubscriptionByIdAsync(subscriptionId);
        if (subscription == null)
        {
            throw new NotFoundException($"Subscription of id: {subscriptionId} does not exist");
        }
        
        subscription.ProcessPayment(requestModel.Amount);
        subscriptionsRepository.UpdateSubscriptionAsync(subscription);
    }
}