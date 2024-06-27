using APBD_Projekt.Exceptions;
using APBD_Projekt.Helpers;
using APBD_Projekt.Models;
using Microsoft.IdentityModel.Tokens;
using Xunit.Abstractions;

namespace APBD_Projekt.Tests.Models;

public class UserTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void UpdateRefreshToken_ShouldNotBeSameAsOld()
    {
        // Arrange
        const string password = "kot";
        var refreshToken = SecurityHelpers.GenerateRefreshToken();
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(password);
        var user = new User(
            "ala",
            hashedPasswordAndSalt.Item1,
            hashedPasswordAndSalt.Item2,
            refreshToken,
            DateTime.Now.AddDays(1),
            null!);

        // Act
        user.UpdateRefreshToken();
        var newRefreshToken = user.RefreshToken;

        // Assert
        Assert.NotNull(newRefreshToken);
        Assert.NotEqual(refreshToken, newRefreshToken);
    }

    [Fact]
    public void EnsureUsersRefreshTokenMatchesAndIsValid_ShouldThrowSecurityTokenException_WhenTokenExpired()
    {
        // Arrange
        const string password = "kot";
        var refreshToken = SecurityHelpers.GenerateRefreshToken();
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(password);
        var user = new User(
            "ala",
            hashedPasswordAndSalt.Item1,
            hashedPasswordAndSalt.Item2,
            refreshToken,
            DateTime.Now.AddYears(-10),
            null!);

        // Act & Assert
        var e = Assert.Throws<SecurityTokenException>(() =>
            user.EnsureUsersRefreshTokenMatchesAndIsValid(refreshToken));
        testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public void EnsureUsersRefreshTokenMatchesAndIsValid_ShouldNotThrow_WhenTokenCorrect()
    {
        // Arrange
        const string password = "kot";
        var refreshToken = SecurityHelpers.GenerateRefreshToken();
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(password);
        var user = new User(
            "ala",
            hashedPasswordAndSalt.Item1,
            hashedPasswordAndSalt.Item2,
            refreshToken,
            DateTime.Now.AddDays(1),
            null!);

        // Act & Assert
        try
        {
            user.EnsureUsersRefreshTokenMatchesAndIsValid(refreshToken);
            Assert.True(true);
        }
        catch
        {
            Assert.Fail();
        }
    }

    [Fact]
    public void EnsureUsersRefreshTokenMatchesAndIsValid_ShouldThrowSecurityTokenException_WhenTokenIncorrect()
    {
        // Arrange
        const string password = "kot";
        var refreshToken = SecurityHelpers.GenerateRefreshToken();
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(password);
        var user = new User(
            "ala",
            hashedPasswordAndSalt.Item1,
            hashedPasswordAndSalt.Item2,
            refreshToken,
            DateTime.Now.AddDays(1),
            null!);
        const string incorrectRefreshToken = "incorrectToken";

        // Act & Assert
        var e = Assert.Throws<SecurityTokenException>(() =>
            user.EnsureUsersRefreshTokenMatchesAndIsValid(incorrectRefreshToken));
        testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public void EnsurePasswordIsValid_ShouldThrowException_WhenPasswordIncorrect()
    {
        // Arrange
        const string password = "kot";
        const string incorrectPassword = "incPass";
        var refreshToken = SecurityHelpers.GenerateRefreshToken();
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(password);
        var user = new User(
            "ala",
            hashedPasswordAndSalt.Item1,
            hashedPasswordAndSalt.Item2,
            refreshToken,
            DateTime.Now.AddDays(1),
            null!);

        // Act & Assert
        var e = Assert.Throws<UnauthorizedException>(() =>
            user.EnsurePasswordIsValid(incorrectPassword));
        testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public void EnsurePasswordIsValid_ShouldNotThrowException_WhenPasswordIncorrect()
    {
        // Arrange
        const string password = "kot";
        var refreshToken = SecurityHelpers.GenerateRefreshToken();
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(password);
        var user = new User(
            "ala",
            hashedPasswordAndSalt.Item1,
            hashedPasswordAndSalt.Item2,
            refreshToken,
            DateTime.Now.AddDays(1),
            null!);

        // Act & Assert
        try
        {
            user.EnsurePasswordIsValid(password);
            Assert.True(true);
        }
        catch (Exception e)
        {
            Assert.Fail(e.Message);
        }
    }
}