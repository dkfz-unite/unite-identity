using System.Linq.Expressions;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Services;

namespace Unite.Identity.Services;

public class ProviderService
{
    private readonly IdentityDbContext _dbContext;

    public ProviderService(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public Provider Get(int id)
    {
        return _dbContext.Set<Provider>()
            .FirstOrDefault(provider => provider.Id == id);
    }

    public Provider Get(Expression<Func<Provider, bool>> predicate)
    {
        return _dbContext.Set<Provider>()
            .FirstOrDefault(predicate);
    }

    public Provider[] GetAll(Expression<Func<Provider, bool>> predicate)
    {
        return _dbContext.Set<Provider>()
            .Where(predicate)
            .ToArray();
    }

    public Provider Add(string name, string label, bool isActive, int? priority)
    {
        var entity = Get(provider => provider.Name == name);
        if (entity != null)
            return null;

        entity = new Provider
        {
            Name = name,
            Label = label,
            IsActive = isActive,
            Priority = priority
        };

        _dbContext.Add(entity);
        _dbContext.SaveChanges();

        return entity;
    }

    public Provider Update(int id, string name, string label, bool isActive, int? priority)
    {
        var entity = Get(id);
        if (entity == null)
            return null;

        entity.Name = name;
        entity.Label = label;
        entity.IsActive = isActive;
        entity.Priority = priority;

        _dbContext.Update(entity);
        _dbContext.SaveChanges();

        return entity;
    }

    public bool Delete(int id)
    {
        var entity = Get(id);
        if (entity == null)
            return false;

        Delete(entity);
        return true;
    }

    public void Delete(Provider entity)
    {
        _dbContext.Remove(entity);
        _dbContext.SaveChanges();
    }
}

