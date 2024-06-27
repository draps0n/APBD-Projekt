using APBD_Projekt.Models;
using APBD_Projekt.Repositories.Abstractions;

namespace APBD_Projekt.Tests.TestObjects;

public class FakeUsersRepository(List<User> users, List<Role> roles) : IUsersRepository
{
    public async Task RegisterUserAsync(User user)
    {
        users.Add(user);
    }

    public async Task<Role?> GetRoleByNameAsync(string roleName)
    {
        return roles
            .FirstOrDefault(r => r.Name == roleName);
    }

    public async Task<User?> GetUserWithRoleByLoginAsync(string login)
    {
        return users
            .FirstOrDefault(u => u.Login == login);
    }

    public async void UpdateUser(User user)
    {
        var userInDb = users.FirstOrDefault(u => u.IdUser == user.IdUser);
        users.Remove(userInDb!);
        users.Add(user);
    }

    public async Task CreateRoleAsync(Role role)
    {
        roles.Add(role);
    }

    public async Task SaveChangesAsync()
    {
    }
}