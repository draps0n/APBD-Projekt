using System.Reflection;
using APBD_Projekt.Exceptions;
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
        var user = new User("ala", "kot", null!);
        var oldRefreshToken = user.RefreshToken;

        // Act
        user.UpdateRefreshToken();
        var newRefreshToken = user.RefreshToken;

        // Assert
        Assert.NotNull(newRefreshToken);
        Assert.NotEqual(oldRefreshToken, newRefreshToken);
    }

    [Fact]
    public void EnsureUsersRefreshTokenMatchesAndIsValid_ShouldThrowSecurityTokenException_WhenTokenExpired()
    {
        // Arrange
        var user = new User("ala", "kot", null!);
        var refreshToken = user.RefreshToken;
        var property = user.GetType().GetProperty("RefreshTokenExp",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!;
        property.SetValue(user, DateTime.Now.AddYears(-10));

        // Act & Assert
        var e = Assert.Throws<SecurityTokenException>(() =>
            user.EnsureUsersRefreshTokenMatchesAndIsValid(refreshToken));
        testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public void EnsureUsersRefreshTokenMatchesAndIsValid_ShouldNotThrow_WhenTokenCorrect()
    {
        // Arrange
        var user = new User("ala", "kot", null!);
        var refreshToken = user.RefreshToken;

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
        var user = new User("ala", "kot", null!);
        const string refreshToken = "incorrectToken";

        // Act & Assert
        var e = Assert.Throws<SecurityTokenException>(() =>
            user.EnsureUsersRefreshTokenMatchesAndIsValid(refreshToken));
        testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public void EnsurePasswordIsValid_ShouldThrowException_WhenPasswordIncorrect()
    {
        // Arrange
        const string password = "kot";
        var user = new User("ala", password, null!);

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

    [Fact]
    public void EnsurePasswordIsValid_ShouldNotThrowException_WhenPasswordcorrect()
    {
        // Arrange
        var user = new User("ala", "kot", null!);

        // Act & Assert
        var e = Assert.Throws<UnauthorizedException>(() =>
            user.EnsurePasswordIsValid(user.Password));
        testOutputHelper.WriteLine(e.Message);
    }
}