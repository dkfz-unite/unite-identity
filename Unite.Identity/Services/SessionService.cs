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

    public UserSession CreateSession(User user, string client, DateTime expiryDate)
    {
        var session = Guid.NewGuid().ToString();

        var userSession = new UserSession()
        {
            UserId = user.Id,
            Client = client,
            Session = session,
            Expires = expiryDate
        };

        _dbContext.Add(userSession);
        _dbContext.SaveChanges();

        return userSession;
    }

    public UserSession FindSession(User user, string session)
    {
        var userSession = _dbContext.Set<UserSession>()
            .FirstOrDefault(userSession =>
                userSession.UserId == user.Id &&
                userSession.Session == session
            );

        return userSession;
    }

    public string RotateSession(UserSession session)
    {
        session.Session = Guid.NewGuid().ToString();
        _dbContext.Update(session);
        _dbContext.SaveChanges();

        return session.Session;
    }

    public void RemoveSession(UserSession session)
    {
        _dbContext.Remove(session);
        _dbContext.SaveChanges();
    }
}
