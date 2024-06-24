using APBD_Projekt.ResponseModels;

namespace APBD_Projekt.Services;

public interface IUsersService
{
    Task RegisterUserAsync(string login, string password);
    Task<LoginUserResponseModel> LoginUserAsync(string login, string password);
    Task<RefreshTokenResponseModel> RefreshUserTokenAsync(string? login, string refreshToken);
}