using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APBD_Projekt.Exceptions;
using APBD_Projekt.Models;
using APBD_Projekt.Repositories.Abstractions;
using APBD_Projekt.ResponseModels;
using APBD_Projekt.Services.Abstractions;
using Microsoft.IdentityModel.Tokens;

namespace APBD_Projekt.Services;

public class UsersService(IUsersRepository usersRepository, IConfiguration configuration) : IUsersService
{
    public async Task RegisterUserAsync(string login, string password)
    {
        await EnsureLoginIsUniqueAsync(login);
        var standardUserRole = await GetOrAddStandardRoleAsync();

        var user = new User(
            login,
            password,
            standardUserRole
        );

        await usersRepository.RegisterUserAsync(user);
    }

    public async Task<LoginUserResponseModel> LoginUserAsync(string login, string password)
    {
        var user = await usersRepository.GetUserByLoginAsync(login);

        if (user is null)
        {
            throw new UnauthorizedException("Invalid login or password");
        }

        user.EnsurePasswordIsValid(password);

        var accessToken = GenerateJwtTokenForUser(user);
        user.UpdateRefreshToken();

        // TODO: unit of work pattern
        await usersRepository.UpdateUserRefreshTokenAsync(user);

        return new LoginUserResponseModel
        {
            JwtToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = user.RefreshToken
        };
    }

    public async Task<RefreshTokenResponseModel> RefreshUserTokenAsync(string? login, string refreshToken)
    {
        if (login is null)
        {
            throw new SecurityTokenException("Invalid access token");
        }

        var user = await usersRepository.GetUserByLoginAsync(login);
        if (user is null)
        {
            throw new SecurityTokenException("Invalid access token");
        }

        user.EnsureUsersRefreshTokenMatchesAndIsValid(refreshToken);

        var token = GenerateJwtTokenForUser(user);
        user.UpdateRefreshToken();
        await usersRepository.UpdateUserRefreshTokenAsync(user);

        return new RefreshTokenResponseModel
        {
            JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = user.RefreshToken
        };
    }

    private JwtSecurityToken GenerateJwtTokenForUser(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.Role.Name)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]!));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: configuration["JWT:Issuer"],
            audience: configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: signingCredentials
        );

        return token;
    }

    private async Task<Role> GetOrAddStandardRoleAsync()
    {
        var standardUserRole = await usersRepository.GetRoleByNameAsync("standard");

        if (standardUserRole is null)
        {
            standardUserRole = new Role("standard");
            await usersRepository.CreateRoleAsync(standardUserRole);
        }

        return standardUserRole;
    }

    private async Task EnsureLoginIsUniqueAsync(string login)
    {
        if (await usersRepository.GetUserByLoginAsync(login) != null)
        {
            throw new BadRequestException($"User of {login} already exists");
        }
    }
}