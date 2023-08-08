using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Unite.Identity.Constants;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Entities.Enums;
using Unite.Identity.Data.Services;

namespace Unite.Identity.Services;

public class WorkerService
{
    private readonly IdentityDbContext _dbContext;

    public WorkerService(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Worker Get(int id)
    {
        return _dbContext.Set<Worker>()
            .Include(entity => entity.WorkerPermissions)
            .FirstOrDefault(entity => entity.Id == id);
    }

    public Worker Get(Expression<Func<Worker, bool>> predicate)
    {
        return _dbContext.Set<Worker>()
            .Include(entity => entity.WorkerPermissions)
            .FirstOrDefault(predicate);
    }

    public Worker[] GetAll()
    {
        return _dbContext.Set<Worker>()
            .Include(entity => entity.WorkerPermissions)
            .ToArray();
    }

    public Worker[] GetAll(Expression<Func<Worker, bool>> predicate)
    {
        return _dbContext.Set<Worker>()
            .Include(entity => entity.WorkerPermissions)
            .Where(predicate)
            .ToArray();
    }

    public Worker Add(string name, string description = null, string token = null, DateTime? tokenExpiryDate = null, Permission[] permissions = null)
    {
        var model = new Worker
        {
            Name = name,
            Description = description,
            Token = token,
            TokenExpiryDate = tokenExpiryDate,
            WorkerPermissions = GetServicePermissions(permissions)
        };

        return Add(model);
    }

    public Worker Add(Worker model)
    {
        var entity = Get(entity => entity.Name == model.Name);

        if (entity == null)
        {
            entity = new Worker();

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

    public Worker Update(int id, Worker model)
    {
        var entity = Get(entity => entity.Id == id && entity.Name != model.Name);

        if (entity != null)
        {
            Map(model, ref entity);

            _dbContext.Update(entity);
            _dbContext.SaveChanges();

            return Get(entity.Id);
        }
        else
        {
            return null;
        }
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


    private static WorkerPermission[] GetServicePermissions(Permission[] permissions = null)
    {
        var defaultPermissions = Permissions.DefaultPermissions;

        return permissions != null && permissions.Any()
            ? permissions.Select(permissionId => new WorkerPermission { PermissionId = permissionId }).ToArray()
            : defaultPermissions.Select(permissionId => new WorkerPermission { PermissionId = permissionId }).ToArray();
    }

    private static void Map(in Worker source, ref Worker target)
    {
        target.Name = source.Name;
        target.Description = source.Description;
        target.Token = source.Token;
        target.TokenExpiryDate = source.TokenExpiryDate;
        target.WorkerPermissions = source.WorkerPermissions;
    }
}
