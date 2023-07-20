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


    public Provider GetProvider(int id)
    {
        return _dbContext.Set<Provider>()
            .FirstOrDefault(provider => provider.Id == id);
    }

    public Provider GetProvider(Expression<Func<Provider, bool>> predicate)
    {
        return _dbContext.Set<Provider>()
            .FirstOrDefault(predicate);
    }

    public Provider[] GetProviders()
    {
        return _dbContext.Set<Provider>()
            .ToArray();
    }

    public Provider[] GetProviders(Expression<Func<Provider, bool>> predicate)
    {
        return _dbContext.Set<Provider>()
            .Where(predicate)
            .ToArray();
    }

    public Provider Add(string name, string label, bool isActive, int? priority)
    {
        var provider = GetProvider(provider => provider.Name == name);

        if (provider == null)
        {
            provider = new Provider
            {
                Name = name,
                Label = label,
                IsActive = isActive,
                Priority = priority
            };

            _dbContext.Add(provider);
            _dbContext.SaveChanges();

            return provider;
        }
        else
        {
            return null;
        }
    }

    public Provider Update(int id, string name, string label, bool isActive, int? priority)
    {
        var provider = GetProvider(id);

        if (provider != null)
        {
            provider.Name = name;
            provider.Label = label;
            provider.IsActive = isActive;
            provider.Priority = priority;

            _dbContext.Update(provider);
            _dbContext.SaveChanges();

            return provider;
        }
        else
        {
            return null;
        }
    }

    public bool Delete(int id)
    {
        var provider = GetProvider(id);

        if (provider != null)
        {
            _dbContext.Remove(provider);
            _dbContext.SaveChanges();

            return true;
        }
        else
        {
            return false;
        }
    }
}

