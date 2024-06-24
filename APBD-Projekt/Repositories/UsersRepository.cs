using APBD_Projekt.Context;
using APBD_Projekt.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_Projekt.Repositories;

public class UsersRepository(DatabaseContext context) : IUsersRepository
{
    public async Task RegisterUserAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task<Role?> GetRoleByNameAsync(string roleName)
    {
        return await context.Roles
            .Where(r => r.Name == roleName)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByLoginAsync(string login)
    {
        return await context.Users
            .Include(u => u.Role)
            .Where(u => u.Login == login)
            .FirstOrDefaultAsync();
    }

    public async Task UpdateUserRefreshTokenAsync(User user)
    {
        await context.SaveChangesAsync();
    }

    public async Task CreateRoleAsync(Role role)
    {
        await context.Roles.AddAsync(role);
        await context.SaveChangesAsync();
    }
}