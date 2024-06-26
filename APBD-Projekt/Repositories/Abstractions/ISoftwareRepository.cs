using APBD_Projekt.Models;

namespace APBD_Projekt.Repositories.Abstractions;

public interface ISoftwareRepository
{
    Task<SoftwareVersion?> GetSoftwareVersionByNameAndVersion(string softwareName, string softwareVersion);
    Task<Software?> GetSoftwareByIdAsync(int softwareId);
}