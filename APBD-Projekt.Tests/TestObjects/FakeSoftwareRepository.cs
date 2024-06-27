using APBD_Projekt.Models;
using APBD_Projekt.Repositories.Abstractions;

namespace APBD_Projekt.Tests.TestObjects;

public class FakeSoftwareRepository(
    List<SoftwareVersion> softwareVersions,
    List<Software> software,
    List<SubscriptionOffer> subscriptionOffers) : ISoftwareRepository
{
    public async Task<SoftwareVersion?> GetSoftwareVersionWithSoftwareByNameAndVersionAsync(string softwareName,
        string softwareVersion)
    {
        return softwareVersions
            .FirstOrDefault(sv => sv.Software.Name == softwareName && sv.Version == softwareVersion);
    }

    public async Task<Software?> GetSoftwareByIdAsync(int softwareId)
    {
        return software
            .FirstOrDefault(s => s.IdSoftware == softwareId);
    }

    public async Task<SubscriptionOffer?> GetSoftwareSubscriptionOfferWithSoftwareByNameAsync(string softwareName,
        string subscriptionOfferName)
    {
        return subscriptionOffers
            .FirstOrDefault(subOff => subOff.Name == subscriptionOfferName && subOff.Software.Name == softwareName);
    }
}