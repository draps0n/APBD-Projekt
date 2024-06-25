using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    public User(string login, string password, string salt, string refreshToken, DateTime? refreshTokenExp, Role role)
    {
        Login = login;
        Password = password;
        Salt = salt;
        RefreshToken = refreshToken;
        RefreshTokenExp = refreshTokenExp;
        Role = role;
    }

    /// <summary>
    /// Updates the refresh token for the user.
    /// </summary>
    /// <remarks>
    /// This method generates a new refresh token using the <see cref="SecurityHelpers.GenerateRefreshToken"/> method,
    /// sets the <see cref="RefreshToken"/> property to the newly generated token, and sets the <see cref="RefreshTokenExp"/>
    /// property to the current date and time plus one day.
    /// </remarks>
    public void UpdateRefreshToken()
    {
        var refreshToken = SecurityHelpers.GenerateRefreshToken();
        RefreshToken = refreshToken;
        RefreshTokenExp = DateTime.Now.AddDays(1);
    }

    /// <summary>
    /// Ensures that the user's refresh token matches the provided refresh token and is valid.
    /// </summary>
    /// <param name="refreshToken">The refresh token to compare against the user's refresh token.</param>
    /// <exception cref="SecurityTokenException">Thrown if the user's refresh token has expired or if the provided refresh token is invalid.</exception>
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

    /// <summary>
    /// Ensures that the provided password is valid for the user.
    /// </summary>
    /// <param name="password">The password to validate.</param>
    /// <exception cref="UnauthorizedException">Thrown if the provided password is invalid.</exception>
    public void EnsurePasswordIsValid(string password)
    {
        var givenPasswordHash = SecurityHelpers.GetHashedPasswordWithSalt(password, Salt);

        if (givenPasswordHash != Password)
        {
            throw new UnauthorizedException("Invalid login or password");
        }
    }
}