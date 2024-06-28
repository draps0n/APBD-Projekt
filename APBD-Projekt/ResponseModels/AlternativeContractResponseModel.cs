namespace APBD_Projekt.ResponseModels;

public class AlternativeContractResponseModel
{
    public string Message { get; set; }
    public int ContractId { get; set; }
    public int ClientId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int YearsOfSupport { get; set; }
    public decimal FinalPrice { get; set; }
    public string SoftwareName { get; set; }
    public string SoftwareVersion { get; set; }
}