namespace APBD_Projekt.Models;

public class ContractPayment
{
    public int IdContractPayment { get; private set; }
    public int IdContract { get; private set; }
    public decimal PaymentAmount { get; private set; }
    public DateTime DateTime { get; private set; }

    public Contract Contract { get; private set; }

    protected ContractPayment()
    {
    }

    public ContractPayment(decimal paymentAmount, DateTime dateTime, Contract contract)
    {
        PaymentAmount = paymentAmount;
        DateTime = dateTime;
        Contract = contract;
    }
}