using APBD_Projekt.Models;
using APBD_Projekt.Persistence;
using APBD_Projekt.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace APBD_Projekt.Repositories;

public class SoftwareRepository(DatabaseContext context) : ISoftwareRepository
{
    public async Task<SoftwareVersion?> GetSoftwareVersionByNameAndVersionAsync(string softwareName, string softwareVersion)
    {
        return await context.SoftwareVersions
            .Include(sv => sv.Software)
            .Where(sv =>
                sv.Software.Name == softwareName && sv.Version == softwareVersion)
            .FirstOrDefaultAsync();
    }

    public async Task<Software?> GetSoftwareByIdAsync(int softwareId)
    {
        return await context.Software
            .Where(s => s.IdSoftware == softwareId)
            .FirstOrDefaultAsync();
    }

    public async Task<SubscriptionOffer?> GetSoftwareSubscriptionOfferByNameAsync(string softwareName,
        string subscriptionOfferName)
    {
        return await context.SubscriptionOffers
            .Include(subOff => subOff.Software)
            .Where(subOff => subOff.Name == subscriptionOfferName && subOff.Software.Name == softwareName)
            .FirstOrDefaultAsync();
    }
}