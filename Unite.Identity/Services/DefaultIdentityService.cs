﻿using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Services;

namespace Unite.Identity.Services;

public class DefaultIdentityService : BaseIdentityService, IIdentityService
{
    public DefaultIdentityService(IdentityDbContext dbContext) : base(dbContext)
    {
    }

    public User RegisterUser(string email, string password, bool isRoot = false)
    {
        var passwordHash = GetStringHash(password);

        var user = _dbContext.Set<User>().FirstOrDefault(user =>
            user.Email == email &&
            user.IsActive == false
        );

        if (user != null)
        {
            user.Password = passwordHash;
            user.IsRoot = isRoot;
            user.IsActive = true;

            _dbContext.Update(user);
            _dbContext.SaveChanges();

            return user;
        }

        return user;
    }


    public User LoginUser(string email, string password)
    {
        var passwordHash = GetStringHash(password);

        var user = _dbContext.Set<User>()
            .Include(user => user.UserPermissions)
            .FirstOrDefault(user =>
                user.Email == email &&
                user.Password == passwordHash
            );

        return user;
    }

    public User ChangePassword(string email, string oldPassword, string newPassword)
    {
        var passwordHash = GetStringHash(newPassword);

        var user = LoginUser(email, oldPassword);

        if (user != null)
        {
            user.Password = passwordHash;

            _dbContext.Update(user);
            _dbContext.SaveChanges();
        }

        return user;
    }

    private static string GetStringHash(string value)
    {
        var md5 = MD5.Create();

        var bytes = Encoding.ASCII.GetBytes(value);
        var hash = md5.ComputeHash(bytes);
        var hashString = Encoding.ASCII.GetString(hash);

        return hashString;
    }
}
