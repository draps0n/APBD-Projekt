using APBD_Projekt.RequestModels;
using APBD_Projekt.ResponseModels;

namespace APBD_Projekt.Services.Abstractions;

public interface ISubscriptionsService
{
    Task<CreateSubscriptionResponseModel> CreateSubscriptionAsync(int clientId, CreateSubscriptionRequestModel requestModel);
    Task PayForSubscriptionAsync(int clientId, int subscriptionId, PayForSubscriptionRequestModel requestModel);
}