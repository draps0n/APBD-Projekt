namespace APBD_Projekt.ResponseModels;

public class RefreshTokenResponseModel
{
    public required string JwtToken { get; set; }
    public required string RefreshToken { get; set; }
}