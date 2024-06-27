using APBD_Projekt.Models;

namespace APBD_Projekt.Repositories.Abstractions;

public interface ISoftwareRepository
{
    Task<SoftwareVersion?> GetSoftwareVersionWithSoftwareByNameAndVersionAsync(string softwareName, string softwareVersion);
    Task<Software?> GetSoftwareByIdAsync(int softwareId);
    Task<SubscriptionOffer?> GetSoftwareSubscriptionOfferWithSoftwareByNameAsync(string softwareName, string subscriptionOfferName);
}