using APBD_Projekt.Models;

namespace APBD_Projekt.Repositories.Abstractions;

public interface ISoftwareRepository
{
    Task<SoftwareVersion?> GetSoftwareVersionByNameAndVersionAsync(string softwareName, string softwareVersion);
    Task<Software?> GetSoftwareByIdAsync(int softwareId);
    Task<SubscriptionOffer?> GetSoftwareSubscriptionOfferByNameAsync(string softwareName, string subscriptionOfferName);
}