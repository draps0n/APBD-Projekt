using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using APBD_Projekt.Exceptions;
using APBD_Projekt.Helpers;
using APBD_Projekt.Models;
using APBD_Projekt.Repositories;
using APBD_Projekt.ResponseModels;
using Microsoft.IdentityModel.Tokens;

namespace APBD_Projekt.Services;

public class UsersService(IUsersRepository usersRepository, IConfiguration configuration) : IUsersService
{
    public async Task RegisterUserAsync(string login, string password)
    {
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(password);

        var standardUserRole = await usersRepository.GetRoleByNameAsync("standard");

        if (standardUserRole is null)
        {
            standardUserRole = new Role { Name = "standard" };
            await usersRepository.CreateRoleAsync(standardUserRole);
        }

        if (await usersRepository.GetUserByLoginAsync(login) != null)
        {
            throw new BadRequestException($"User of {login} already exists");
        }

        var user = new User
        {
            Login = login,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelpers.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(1),
            Role = standardUserRole
        };

        await usersRepository.RegisterUserAsync(user);
    }

    public async Task<LoginUserResponseModel> LoginUserAsync(string login, string password)
    {
        var user = await usersRepository.GetUserByLoginAsync(login);

        if (user is null)
        {
            throw new UnauthorizedException("Invalid login or password");
        }

        var hashedPasswordWithSalt = user.Password;
        var userSalt = user.Salt;
        var currentPasswordHash = SecurityHelpers.GetHashedPasswordWithSalt(password, userSalt);

        if (currentPasswordHash != hashedPasswordWithSalt)
        {
            Console.WriteLine(currentPasswordHash);
            Console.WriteLine(hashedPasswordWithSalt);
            throw new UnauthorizedException("Invalid login or password");
        }

        var token = GenerateJwtTokenForUser(user);

        var refreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        await usersRepository.UpdateUserRefreshTokenAsync(user);

        return new LoginUserResponseModel
        {
            JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken
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

        CheckIfUsersRefTokenMatchAndIsValid(user, refreshToken);

        var token = GenerateJwtTokenForUser(user);

        await UpdateUsersRefToken(user);

        return new RefreshTokenResponseModel
        {
            JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = user.RefreshToken
        };
    }

    private async Task UpdateUsersRefToken(User user)
    {
        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        await usersRepository.UpdateUserRefreshTokenAsync(user);
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

    private static void CheckIfUsersRefTokenMatchAndIsValid(User user, string refreshToken)
    {
        if (user.RefreshTokenExp < DateTime.Now)
        {
            throw new SecurityTokenException("Refresh token expired");
        }

        if (user.RefreshToken != refreshToken)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }
    }
}