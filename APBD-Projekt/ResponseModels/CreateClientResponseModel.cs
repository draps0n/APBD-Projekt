namespace APBD_Projekt.ResponseModels;

public class CreateClientResponseModel
{
    public int IdClient { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string ClientType { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? PESEL { get; set; }
    public string? CompanyName { get; set; }
    public string? KRS { get; set; }
}