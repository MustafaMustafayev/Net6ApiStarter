﻿using Project.DAL.GenericRepositories.Abstract;
using Project.Entity.Entities;

namespace Project.DAL.Abstract;

public interface IUserRepository : IGenericRepository<User>
{
    Task<bool> IsUserExistAsync(string userName, int? userId);

    Task<string> GetUserSaltAsync(string userName);

    Task UpdateUserAsync(User user);

    Task ResetPasswordAsync(int userId, string password);
}