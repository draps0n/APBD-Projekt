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

    public User(string login, string password, Role role)
    {
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(password);
        Login = login;
        Password = hashedPasswordAndSalt.Item1;
        Salt = hashedPasswordAndSalt.Item2;
        RefreshToken = SecurityHelpers.GenerateRefreshToken();
        RefreshTokenExp = DateTime.Now.AddDays(1);
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
        var givenPasswordHash = SecurityHelpers.GetHashedPasswordWithSalt(password, Salt);

        if (givenPasswordHash != Password)
        {
            throw new UnauthorizedException("Invalid login or password");
        }
    }
}