using APBD_Projekt.Models;
using APBD_Projekt.Persistence;
using APBD_Projekt.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace APBD_Projekt.Repositories;

public class UsersRepository(DatabaseContext context) : IUsersRepository
{
    public async Task RegisterUserAsync(User user)
    {
        await context.Users.AddAsync(user);
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

    public void UpdateUser(User user)
    {
        context.Users.Update(user);
    }

    public async Task CreateRoleAsync(Role role)
    {
        await context.Roles.AddAsync(role);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}