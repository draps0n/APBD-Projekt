using APBD_Projekt.Models;
using APBD_Projekt.Persistence;
using APBD_Projekt.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace APBD_Projekt.Repositories;

public class SoftwareRepository(DatabaseContext context) : ISoftwareRepository
{
    public async Task<SoftwareVersion?> GetSoftwareVersionByNameAndVersion(string softwareName, string softwareVersion)
    {
        return await context.SoftwareVersions
            .Include(sv => sv.Software)
            .Where(sv =>
                sv.Software.Name == softwareName && sv.Version == softwareVersion)
            .FirstOrDefaultAsync();
    }

    public Task<Software?> GetSoftwareByIdAsync(int softwareId)
    {
        return context.Software
            .Where(s => s.IdSoftware == softwareId)
            .FirstOrDefaultAsync();
    }
}