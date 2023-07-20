using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Services;

namespace Unite.Identity.Services;

public abstract class BaseIdentityService
{
    protected readonly IdentityDbContext _dbContext;
    protected readonly UserService _userService;

    public BaseIdentityService(IdentityDbContext dbContext, UserService userService)
    {
        _dbContext = dbContext;
        _userService = userService;
    }

    public User GetUser(string provider, string email, bool isActive)
    {
        return _userService.GetUser(user => 
            user.Provider.Name == provider && 
            user.Email == email && 
            user.IsActive == isActive
        );
    }
}
