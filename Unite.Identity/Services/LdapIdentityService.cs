using Unite.Identity.Constants;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Services;
using Unite.Identity.Services.Ldap;

namespace Unite.Identity.Services;

public class LdapIdentityService : BaseIdentityService, IIdentityService
{
    private readonly LdapService _ldapService;

    public LdapIdentityService(IdentityDbContext dbContext, UserService userService, LdapService ldapService) : base(dbContext, userService)
    {
        _ldapService = ldapService;
    }

    public User LoginUser(string login, string password)
    {
        var ldapUser = _ldapService.FindUser(login);

        if (ldapUser == null)
        {
            return null;
        }

        var uniteUser = GetUser(Providers.Ldap, ldapUser.Email, true);

        if (uniteUser == null)
        {
            return null;
        }

        var authenticated = _ldapService.AuthenticateUser(ldapUser.Login, password);

        return authenticated ? uniteUser : null;
    }
}
