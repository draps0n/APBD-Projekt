namespace APBD_Projekt.Models;

public class Software
{
    public int IdSoftware { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal YearlyLicensePrice { get; private set; }

    public ICollection<Category> Categories { get; private set; }
    public ICollection<SoftwareVersion> SoftwareVersions { get; private set; }
    public ICollection<SubscriptionOffer> SubscriptionOffers { get; private set; }

    protected Software()
    {
    }

    public Software(string name, string description, decimal yearlyLicensePrice)
    {
        Name = name;
        Description = description;
        YearlyLicensePrice = yearlyLicensePrice;
    }
}