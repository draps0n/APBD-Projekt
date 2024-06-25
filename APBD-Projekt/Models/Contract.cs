namespace APBD_Projekt.Models;

public class Contract
{
    public int IdContract { get; private set; }
    public int IdClient { get; private set; }
    public int IdSoftwareVersion { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public int YearsOfSupport { get; private set; }
    public decimal FinalPrice { get; private set; }
    public int? IdDiscount { get; private set; }

    public Client Client { get; private set; }
    public SoftwareVersion SoftwareVersion { get; private set; }
    public Discount? Discount { get; private set; }
    public ICollection<ContractPayment> ContractPayments { get; private set; }

    protected Contract()
    {
    }

    public Contract(DateTime startDate, DateTime endDate, int yearsOfSupport, decimal finalPrice, Client client,
        SoftwareVersion softwareVersion, Discount? discount = null)
    {
        StartDate = startDate;
        EndDate = endDate;
        YearsOfSupport = yearsOfSupport;
        FinalPrice = finalPrice;
        Client = client;
        SoftwareVersion = softwareVersion;
        Discount = discount;
    }
}