using APBD_Projekt.Exceptions;

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
    public DateTime? SignedAt { get; private set; }

    public Client Client { get; private set; }
    public SoftwareVersion SoftwareVersion { get; private set; }
    public Discount? Discount { get; private set; }
    public ICollection<ContractPayment> ContractPayments { get; private set; }

    protected Contract()
    {
    }

    public Contract(DateTime startDate, DateTime endDate, int? yearsOfAdditionalSupport, Client client,
        SoftwareVersion softwareVersion, Discount? discount = null)
    {
        EnsureContractTimespanIsValid(startDate, endDate);
        EnsureClientHasNoSuchActiveSoftware(client, softwareVersion);
        var yearsOfSupport = GetYearsOfSupport(yearsOfAdditionalSupport);
        var finalPrice = ApplyAdditionalSupportCost(yearsOfSupport, softwareVersion);
        finalPrice = ApplyDiscounts(client, discount, finalPrice);

        StartDate = startDate;
        EndDate = endDate;
        YearsOfSupport = yearsOfSupport;
        FinalPrice = finalPrice;
        Client = client;
        SoftwareVersion = softwareVersion;
        Discount = discount;
        SignedAt = null;
    }

    private static int GetYearsOfSupport(int? yearsOfAdditionalSupport)
    {
        if (yearsOfAdditionalSupport == null)
        {
            return 1;
        }

        return (int)(1 + yearsOfAdditionalSupport);
    }

    private static decimal ApplyAdditionalSupportCost(int yearsOfSupport,
        SoftwareVersion softwareVersion)
    {
        return softwareVersion.Software.YearlyLicensePrice + (yearsOfSupport - 1) * 1000;
    }

    private static decimal ApplyDiscounts(Client client, Discount? discount, decimal finalPrice)
    {
        if (discount != null)
        {
            finalPrice *= 1 - (decimal)discount.Percentage / 100;
        }

        if (client.IsPreviousClient())
        {
            finalPrice *= (decimal)0.95;
        }

        return finalPrice;
    }

    private static void EnsureClientHasNoSuchActiveSoftware(Client client, SoftwareVersion softwareVersion)
    {
        if (client.HasActiveSoftware(softwareVersion.Software))
        {
            throw new BadRequestException($"Client has an active software: {softwareVersion.Software.Name}");
        }
    }

    private static void EnsureContractTimespanIsValid(DateTime startDate, DateTime endDate)
    {
        var contractLengthInDays = endDate.Subtract(startDate).Days;
        if (contractLengthInDays < 3 || contractLengthInDays > 30)
        {
            throw new BadRequestException("Contract length must be between 3 and 30 days");
        }
    }

    public void ProcessPaymentAndSignIfPossible(decimal paymentAmount)
    {
        var alreadyPaid = ContractPayments.Sum(cp => cp.PaymentAmount);

        if (alreadyPaid + paymentAmount > FinalPrice)
        {
            throw new BadRequestException($"Cannot pay more than it is left to pay: {FinalPrice - alreadyPaid}");
        }

        if (alreadyPaid + paymentAmount == FinalPrice)
        {
            SignedAt = DateTime.Now;
        }
    }

    public bool IsActiveAndForGivenSoftware(int idSoftware)
    {
        return SoftwareVersion.IdSoftware == idSoftware && ((SignedAt == null && DateTime.Now < EndDate) ||
                                                            (SignedAt != null &&
                                                             SignedAt.Value.AddYears(YearsOfSupport) > DateTime.Now));
    }
}