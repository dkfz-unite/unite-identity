using Unite.Identity.Constants;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Services;
using Unite.Identity.Helpers;

namespace Unite.Identity.Services;

public class DefaultIdentityService : BaseIdentityService, IIdentityService
{
    public DefaultIdentityService(IdentityDbContext dbContext, UserService userService) : base(dbContext, userService)
    {
    }

    public User LoginUser(string email, string password)
    {
        var passwordHash = PasswordHelpers.GetPasswordHash(password);

        var user = GetUser(Providers.Default, email, true);

        if (user == null)
        {
            return null;
        }

        var authenticated = user.Password == passwordHash;

        return authenticated ? user : null;
    }
}
