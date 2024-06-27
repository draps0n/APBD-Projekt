using System.Reflection;
using APBD_Projekt.Exceptions;
using APBD_Projekt.Helpers;
using APBD_Projekt.Models;
using APBD_Projekt.Repositories.Abstractions;
using APBD_Projekt.RequestModels;
using APBD_Projekt.Services;
using APBD_Projekt.Tests.TestObjects;
using Castle.Core.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Xunit.Abstractions;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace APBD_Projekt.Tests.Services;

public class UsersServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IConfiguration _configurationMock;
    private const string StandardRoleName = "standard";
    private const string AdminRoleName = "admin";

    public UsersServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        var configurationMock = new Mock<IConfiguration>();

        configurationMock.Setup(config => config["JWT:SecretKey"])
            .Returns("my_super_secret_key_i_love_pjatk");

        configurationMock.Setup(config => config["JWT:Issuer"])
            .Returns("http://localhost:5002/");

        configurationMock.Setup(config => config["JWT:Audience"])
            .Returns("http://localhost:5002/");

        _configurationMock = configurationMock.Object;
        SecurityHelpers.Configure(_configurationMock);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldAddUserAndStandardRole_WhenRequestIsValid()
    {
        // Arrange
        const string login = "adam";
        const string password = "haslo123";
        var fakeUsersRepository = new FakeUsersRepository([], []);
        var usersService = new UsersService(fakeUsersRepository, _configurationMock);

        // Act
        await usersService.RegisterUserAsync(login, password);
        var user = await fakeUsersRepository.GetUserWithRoleByLoginAsync(login);
        var role = await fakeUsersRepository.GetRoleByNameAsync(StandardRoleName);

        // Assert
        Assert.NotNull(user);
        Assert.NotNull(role);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldAddUser_WhenRoleExistsRequestIsValid()
    {
        // Arrange
        const string login = "adam";
        const string password = "haslo123";
        var fakeUsersRepository = new FakeUsersRepository([], [new Role(StandardRoleName)]);
        var usersService = new UsersService(fakeUsersRepository, _configurationMock);

        // Act
        await usersService.RegisterUserAsync(login, password);
        var user = await fakeUsersRepository.GetUserWithRoleByLoginAsync(login);

        // Assert
        Assert.NotNull(user);
    }

    [Fact]
    public async Task RegisterUserAsync_ShouldThrowBadRequestException_WhenLoginNotUnique()
    {
        // Arrange
        const string login = "adam";
        const string password = "haslo123";
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(password);
        var refreshToken = SecurityHelpers.GenerateRefreshToken();
        var user = new User(
            login,
            hashedPasswordAndSalt.Item1,
            hashedPasswordAndSalt.Item2,
            refreshToken,
            DateTime.Now.AddDays(1),
            null!
        );
        var fakeUsersRepository = new FakeUsersRepository([user], []);
        var usersService = new UsersService(fakeUsersRepository, _configurationMock);

        // Act & Assert
        var e = await Assert.ThrowsAsync<BadRequestException>(async () =>
            await usersService.RegisterUserAsync(login, password)
        );
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task LoginUserAsync_ShouldThrowUnauthorizedException_WhenLoginNotExists()
    {
        // Arrange
        const string login = "adam";
        const string password = "haslo123";
        var fakeUsersRepository = new FakeUsersRepository([], []);
        var usersService = new UsersService(fakeUsersRepository, _configurationMock);

        // Act & Assert
        var e = await Assert.ThrowsAsync<UnauthorizedException>(async () =>
            await usersService.LoginUserAsync(login, password)
        );
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task LoginUserAsync_ShouldThrowUnauthorizedException_WhenPasswordDoesNotMatch()
    {
        // Arrange
        const string login = "adam";
        const string password = "haslo123";
        const string incorrectPassword = "incorrectPassword";
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(password);
        var refreshToken = SecurityHelpers.GenerateRefreshToken();
        var user = new User(
            login,
            hashedPasswordAndSalt.Item1,
            hashedPasswordAndSalt.Item2,
            refreshToken,
            DateTime.Now.AddDays(1),
            null!
        );

        var fakeUsersRepository = new FakeUsersRepository([user], []);
        var usersService = new UsersService(fakeUsersRepository, _configurationMock);

        // Act & Assert
        var e = await Assert.ThrowsAsync<UnauthorizedException>(async () =>
            await usersService.LoginUserAsync(login, incorrectPassword)
        );
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task LoginUserAsync_ShouldLoginUser_WhenCredentialsAreValid()
    {
        // Arrange
        const string login = "adam";
        const string password = "haslo123";
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(password);
        var refreshToken = SecurityHelpers.GenerateRefreshToken();
        var user = new User(
            login,
            hashedPasswordAndSalt.Item1,
            hashedPasswordAndSalt.Item2,
            refreshToken,
            DateTime.Now.AddDays(1),
            new Role(StandardRoleName)
        );
        var fakeUsersRepository = new FakeUsersRepository([user], []);
        var usersService = new UsersService(fakeUsersRepository, _configurationMock);

        // Act
        var result = await usersService.LoginUserAsync(login, password);
        var userRefreshToken = user.RefreshToken;

        // Assert
        Assert.Equal(result.RefreshToken, userRefreshToken);
    }

    [Fact]
    public async Task RefreshUserTokenAsync_ShouldThrowSecurityTokenException_WhenRefreshTokenExpired()
    {
        // Arrange
        const string login = "adam";
        const string password = "haslo123";
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(password);
        var refreshToken = SecurityHelpers.GenerateRefreshToken();
        var user = new User(
            login,
            hashedPasswordAndSalt.Item1,
            hashedPasswordAndSalt.Item2,
            refreshToken,
            DateTime.Now.AddDays(1),
            new Role(StandardRoleName)
        );
        var property = user.GetType().GetProperty("RefreshTokenExp",
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!;
        var fakeUsersRepository = new FakeUsersRepository([user], []);
        var usersService = new UsersService(fakeUsersRepository, _configurationMock);

        // Act
        var result = await usersService.LoginUserAsync(login, password);
        var userRefreshToken = user.RefreshToken;
        property.SetValue(user, DateTime.Now.AddYears(-10));

        // Act & Assert
        var e = await Assert.ThrowsAsync<SecurityTokenException>(async () =>
            await usersService.RefreshUserTokenAsync(login, userRefreshToken)
        );
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task RefreshUserTokenAsync_ShouldThrowSecurityTokenException_WhenRefreshTokenIncorrect()
    {
        // Arrange
        const string login = "adam";
        const string password = "haslo123";
        const string incorrectToken = "incorrectToken";
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(password);
        var refreshToken = SecurityHelpers.GenerateRefreshToken();
        var user = new User(
            login,
            hashedPasswordAndSalt.Item1,
            hashedPasswordAndSalt.Item2,
            refreshToken,
            DateTime.Now.AddDays(1),
            new Role(StandardRoleName)
        );
        var fakeUsersRepository = new FakeUsersRepository([user], []);
        var usersService = new UsersService(fakeUsersRepository, _configurationMock);

        // Act
        await usersService.LoginUserAsync(login, password);

        // Act & Assert
        var e = await Assert.ThrowsAsync<SecurityTokenException>(async () =>
            await usersService.RefreshUserTokenAsync(login, incorrectToken)
        );
        _testOutputHelper.WriteLine(e.Message);
    }

    [Fact]
    public async Task RefreshUserTokenAsync_ShouldRefreshToken_WhenRefreshTokenValid()
    {
        // Arrange
        const string login = "adam";
        const string password = "haslo123";
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(password);
        var refreshToken = SecurityHelpers.GenerateRefreshToken();
        var user = new User(
            login,
            hashedPasswordAndSalt.Item1,
            hashedPasswordAndSalt.Item2,
            refreshToken,
            DateTime.Now.AddDays(1),
            new Role(StandardRoleName)
        );
        var fakeUsersRepository = new FakeUsersRepository([user], []);
        var usersService = new UsersService(fakeUsersRepository, _configurationMock);

        // Act
        var result = await usersService.LoginUserAsync(login, password);
        var oldRefreshToken = result.RefreshToken;
        ;
        var refreshTokenResponse = await usersService.RefreshUserTokenAsync(login, oldRefreshToken);
        var newRefreshToken = refreshTokenResponse.RefreshToken;
        var userRefreshToken = user.RefreshToken;

        // Act & Assert
        Assert.NotEqual(newRefreshToken, oldRefreshToken);
        Assert.Equal(newRefreshToken, userRefreshToken);
    }
}