using System.Linq.Expressions;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Services;

namespace Unite.Identity.Services;

public class SessionService
{
    private readonly IdentityDbContext _dbContext;


    public SessionService(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public UserSession Get(int userId, string session)
    {
        return Get(entity =>
            entity.UserId == userId &&
            entity.Session == session
        );
    }

    public UserSession Get(Expression<Func<UserSession, bool>> predicate)
    {
        return _dbContext.Set<UserSession>()
            .FirstOrDefault(predicate);
    }

    public UserSession[] GetAll(Expression<Func<UserSession, bool>> predicate)
    {
        return _dbContext.Set<UserSession>()
            .Where(predicate)
            .ToArray();
    }

    public UserSession Add(int userId, string client, DateTime expiryDate)
    {
        var entity = new UserSession()
        {
            UserId = userId,
            Client = client,
            Session = Guid.NewGuid().ToString(),
            Expires = expiryDate
        };

        _dbContext.Add(entity);
        _dbContext.SaveChanges();

        return entity;
    }

    public string Rotate(UserSession entity)
    {
        entity.Session = Guid.NewGuid().ToString();
        
        _dbContext.Update(entity);
        _dbContext.SaveChanges();

        return entity.Session;
    }

    public void Delete(UserSession entity)
    {
        _dbContext.Remove(entity);
        _dbContext.SaveChanges();
    }

    public void DeleteAll(params UserSession[] entities)
    {
        _dbContext.Remove(entities);
        _dbContext.SaveChanges();
    }

    public void DeleteExpired()
    {
        var entities = _dbContext.Set<UserSession>()
            .Where(entity => entity.Expires < DateTime.UtcNow)
            .ToArray();

        DeleteAll(entities);
    }
}
