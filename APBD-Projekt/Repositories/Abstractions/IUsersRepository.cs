﻿using APBD_Projekt.Models;

namespace APBD_Projekt.Repositories.Abstractions;

public interface IUsersRepository
{
    Task RegisterUserAsync(User user);
    Task<Role?> GetRoleByNameAsync(string roleName);
    Task<User?> GetUserByLoginAsync(string login);
    void UpdateUser(User user);
    Task CreateRoleAsync(Role role);
    Task SaveChangesAsync();
}