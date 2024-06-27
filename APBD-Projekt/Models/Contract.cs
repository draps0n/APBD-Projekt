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
        SignedAt = null;
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

    public void EnsureIsNotAlreadySigned()
    {
        if (SignedAt != null)
        {
            throw new BadRequestException("Cannot pay for contract already signed");
        }
    }

    public bool IsActive()
    {
        return EndDate > DateTime.Now;
    }
}