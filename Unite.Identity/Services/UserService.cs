using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Unite.Identity.Constants;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Entities.Enums;
using Unite.Identity.Data.Services;

namespace Unite.Identity.Services;

public class UserService
{
    private readonly IdentityDbContext _dbContext;


    public UserService(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public User Get(int id)
    {
        return Get(entity => entity.Id == id);
    }

    public User Get(Expression<Func<User, bool>> predicate)
    {
        return _dbContext.Set<User>()
            .Include(entity => entity.Provider)
            .Include(entity => entity.UserPermissions)
            .Include(entity => entity.UserSessions)
            .FirstOrDefault(predicate);
    }

    public User[] GetAll(Expression<Func<User, bool>> predicate)
    {
        return _dbContext.Set<User>()
            .Include(entity => entity.Provider)
            .Include(entity => entity.UserPermissions)
            .Include(entity => entity.UserSessions)
            .Where(predicate)
            .ToArray();
    }

    public User Add(string email, int providerId, bool isActive, bool isRoot, Permission[] permissions = null)
    {
        var entity = Get(user => user.Email == email && user.ProviderId == providerId);
        if (entity != null)
            return null;

        entity = new User
        {
            ProviderId = providerId,
            Email = email,
            IsActive = isActive,
            IsRoot = isRoot,
            UserPermissions = GetUserPermissions(permissions),
            LastActive = DateTime.UtcNow
        };

        _dbContext.Add(entity);
        _dbContext.SaveChanges();

        return Get(entity.Id);
    }

    public User Update(int id, int providerId, Permission[] permissions = null)
    {
        var entity = Get(id);
        if (entity == null)
            return null;

        entity.ProviderId = providerId;
        entity.UserPermissions = GetUserPermissions(permissions);
        entity.LastActive = DateTime.UtcNow;

        _dbContext.Update(entity);
        _dbContext.SaveChanges();

        return Get(entity.Id);
    }

    public void UpdateActivity(User entity)
    {
        entity.LastActive = DateTime.UtcNow;

        _dbContext.Update(entity);
        _dbContext.SaveChanges();
    }

    public bool Delete(int id)
    {
        var entity = Get(id);
        if (entity == null)
            return false;
        
        Delete(entity);
        return true;
    }

    public void Delete(User entity)
    {
        _dbContext.Remove(entity);
        _dbContext.SaveChanges();
    }


    private static UserPermission[] GetUserPermissions(Permission[] permissions = null)
    {
        var defaultPermissions = Permissions.DefaultPermissions;

        return permissions != null && permissions.Any()
            ? permissions.Select(permissionId => new UserPermission { PermissionId = permissionId }).ToArray()
            : defaultPermissions.Select(permissionId => new UserPermission { PermissionId = permissionId }).ToArray();
    }
}
