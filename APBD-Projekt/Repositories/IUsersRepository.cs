using APBD_Projekt.Models;

namespace APBD_Projekt.Repositories;

public interface IUsersRepository
{
    Task RegisterUserAsync(User user);
    Task<Role?> GetRoleByNameAsync(string roleName);
    Task<User?> GetUserByLoginAsync(string login);
    Task UpdateUserRefreshTokenAsync(User user);
    Task CreateRoleAsync(Role role);
}