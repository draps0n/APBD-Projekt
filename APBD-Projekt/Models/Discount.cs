using APBD_Projekt.Enums;

namespace APBD_Projekt.Models;

public class Discount
{
    public int IdDiscount { get; private set; }
    public string Name { get; private set; }
    public DiscountType Type { get; private set; }
    public int Percentage { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public ICollection<Contract> Contracts { get; private set; }
    public ICollection<Subscription> Subscriptions { get; private set; }

    protected Discount()
    {
    }

    public Discount(string name, DiscountType type, int percentage, DateTime startDate, DateTime endDate)
    {
        Name = name;
        Type = type;
        Percentage = percentage;
        StartDate = startDate;
        EndDate = endDate;
    }
}