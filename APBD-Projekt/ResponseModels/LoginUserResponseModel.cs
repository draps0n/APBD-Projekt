namespace APBD_Projekt.ResponseModels;

public class LoginUserResponseModel
{
    public required string JwtToken { get; set; }
    public required string RefreshToken { get; set; }
}