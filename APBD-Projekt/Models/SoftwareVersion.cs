namespace APBD_Projekt.Models;

public class SoftwareVersion
{
    public int IdSoftwareVersion { get; private set; }
    public int IdSoftware { get; private set; }
    public string Version { get; private set; }

    public Software Software { get; private set; }
    public ICollection<Contract> Contracts { get; private set; }

    protected SoftwareVersion()
    {
    }

    public SoftwareVersion(string version, Software software)
    {
        Version = version;
        Software = software;
    }
}