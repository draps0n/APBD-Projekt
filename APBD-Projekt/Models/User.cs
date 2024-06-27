using APBD_Projekt.Exceptions;
using APBD_Projekt.Helpers;
using Microsoft.IdentityModel.Tokens;

namespace APBD_Projekt.Models;

public class User
{
    public int IdUser { get; private set; }
    public string Login { get; private set; }
    public string Password { get; private set; }
    public string Salt { get; private set; }
    public int IdRole { get; private set; }
    public string RefreshToken { get; private set; }
    public DateTime? RefreshTokenExp { get; private set; }

    public Role Role { get; private set; }

    protected User()
    {
    }

    public User(string login, string hashedPassword, string salt, string refreshToken, DateTime? refreshTokenExp,
        Role role)
    {
        Login = login;
        Password = hashedPassword;
        Salt = salt;
        RefreshToken = refreshToken;
        RefreshTokenExp = refreshTokenExp;
        Role = role;
    }

    public void UpdateRefreshToken()
    {
        var refreshToken = SecurityHelpers.GenerateRefreshToken();
        RefreshToken = refreshToken;
        RefreshTokenExp = DateTime.Now.AddDays(1);
    }

    public void EnsureUsersRefreshTokenMatchesAndIsValid(string refreshToken)
    {
        if (RefreshTokenExp < DateTime.Now)
        {
            throw new SecurityTokenException("Refresh token expired");
        }

        if (RefreshToken != refreshToken)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }
    }

    public void EnsurePasswordIsValid(string password)
    {
        var givenPasswordHash = SecurityHelpers.HashPasswordUsingSalt(password, Salt);

        if (givenPasswordHash != Password)
        {
            throw new UnauthorizedException("Invalid login or password");
        }
    }
}