using Microsoft.EntityFrameworkCore;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Services;

namespace Unite.Identity.Services;

public class BaseIdentityService
{
    protected readonly IdentityDbContext _dbContext;

    public BaseIdentityService(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public User GetUser(string email)
    {
        var user = _dbContext.Set<User>()
            .Include(user => user.UserSessions)
            .Include(user => user.UserPermissions)
            .Include(user => user.Provider)
            .FirstOrDefault(user => user.Email == email);

        return user;
    }
}

