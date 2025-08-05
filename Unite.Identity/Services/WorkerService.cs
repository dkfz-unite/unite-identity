using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Unite.Identity.Constants;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Entities.Enums;
using Unite.Identity.Data.Services;

namespace Unite.Identity.Services;

public class TokenService
{
    private readonly IdentityDbContext _dbContext;

    public TokenService(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Token Get(int id)
    {
        return _dbContext.Set<Token>()
            .Include(entity => entity.TokenPermissions)
            .FirstOrDefault(entity => entity.Id == id);
    }

    public Token GetToken(int key)
    {
        return _dbContext.Set<Token>()
            .Include(entity => entity.TokenPermissions)
            .FirstOrDefault(entity => entity.Key == key.ToString()
            && !entity.Revoked
            && entity.TokenExpiryDate > DateTime.UtcNow);
    }

    public Token Get(Expression<Func<Token, bool>> predicate)
    {
        return _dbContext.Set<Token>()
            .Include(entity => entity.TokenPermissions)
            .FirstOrDefault(predicate);
    }

    public Token[] GetAll()
    {
        return _dbContext.Set<Token>()
            .Include(entity => entity.TokenPermissions)
            .ToArray();
    }

    public Token[] GetAll(Expression<Func<Token, bool>> predicate)
    {
        return _dbContext.Set<Token>()
            .Include(entity => entity.TokenPermissions)
            .Where(predicate)
            .ToArray();
    }

    public Token Add(string name, DateTime tokenExpiryDate, Permission[] permissions, string description = null)
    {
        var model = new Token
        {
            Name = name,
            Description = description,
            // Token = token,
            Key = Guid.NewGuid().ToString(),
            TokenExpiryDate = tokenExpiryDate,
            TokenPermissions = GetServicePermissions(permissions)
        };

        return Add(model);
    }

    public Token Add(Token model)
    {
        var entity = Get(entity => entity.Name == model.Name);

        if (entity == null)
        {
            entity = new Token();

            Map(model, ref entity);

            _dbContext.Add(entity);
            _dbContext.SaveChanges();

            return Get(entity.Id);
        }
        else
        {
            return null;
        }
    }

    public Token Update(int id, Token model, Permission[] permissions)
    {
        var entity = Get(entity => entity.Id == id);

        if (entity == null)
            return null;

        var exists = entity.Name != model.Name && _dbContext.Set<Token>().Any(entity => entity.Name == model.Name);
        
        if (exists)
            return null;

        entity.Name = model.Name;
        entity.Description = model.Description;
        entity.TokenExpiryDate = model.TokenExpiryDate;

        if (permissions != null)
        {
            foreach (var permission in entity.TokenPermissions)
            {
                _dbContext.Remove(permission);
            }

            foreach (var permission in GetServicePermissions(permissions))
            {
                entity.TokenPermissions.Add(new TokenPermission
                {
                    TokenId = entity.Id,
                    PermissionId = permission.PermissionId
                });
            }
        }

        _dbContext.Update(entity);
        _dbContext.SaveChanges();

        return Get(entity.Id);
    }

    public Token Update(int id, Token model)
    {
        var entity = Get(entity => entity.Id == id);

        if (entity == null)
            return null;

        var exists = entity.Name != model.Name && _dbContext.Set<Token>().Any(entity => entity.Name == model.Name);
        
        if (exists)
            return null;

        Map(model, ref entity);

        _dbContext.Update(entity);
        _dbContext.SaveChanges();

        return Get(entity.Id);
    }

    public bool Delete(int id)
    {
        var entity = Get(id);

        if (entity != null)
        {
            _dbContext.Remove(entity);
            _dbContext.SaveChanges();

            return true;
        }
        else
        {
            return false;
        }
    }


    private static TokenPermission[] GetServicePermissions(Permission[] permissions = null)
    {
        var defaultPermissions = Permissions.DefaultPermissions;

        return permissions != null && permissions.Any()
            ? permissions.Select(permissionId => new TokenPermission { PermissionId = permissionId }).ToArray()
            : defaultPermissions.Select(permissionId => new TokenPermission { PermissionId = permissionId }).ToArray();
    }

    private static void Map(in Token source, ref Token target)
    {
        target.Name = source.Name;
        target.Description = source.Description;
        // target.Token = source.Token;
        target.Key = source.Key;
        target.Revoked = source.Revoked;
        target.TokenExpiryDate = source.TokenExpiryDate;
        target.TokenPermissions = source.TokenPermissions;
    }
}
