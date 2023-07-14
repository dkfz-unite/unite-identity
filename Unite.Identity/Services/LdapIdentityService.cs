using Unite.Identity.Constants;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Services;
using Unite.Identity.Data.Services.Configuration.Options;
using Unite.Identity.Shared;

namespace Unite.Identity.Services;

public class LdapIdentityService : BaseIdentityService, IIdentityService
{
    private readonly ILdapOptions _options;

    private LDAPAuthentication _ldapAuth;
    private LDAPAuthentication ldapAuth
    {
        get
        {
            if (_ldapAuth == null)
            {
                int? finalPortNumber = null;
                if (_options.Port != null)
                {
                    finalPortNumber = Int16.Parse(_options.Port);
                }

                _ldapAuth = new LDAPAuthentication(_options.Server, _options.ServiceUserRNA, _options.ServiceUserPassword, _options.UserTargetOU, finalPortNumber);
            }

            return _ldapAuth;
        }
    }

    public LdapIdentityService(
        IdentityDbContext dbContext,
        ILdapOptions options) : base(dbContext)
    {
        _options = options;
    }

    public User LoginUser(string userIdentifier, string userPass)
    {
        bool userIdIsEmail = userIdentifier.Contains("@");
        string userMail = userIdentifier;
        if (!userIdIsEmail)
        {
            var result = ldapAuth.ReadUserEntry(userIdentifier);
            if (result != null)
            {
                try
                {
                    userMail = result.Attributes["mail"][0].ToString();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        var user = _dbContext.Set<User>()
            .Include(user => user.UserPermissions)
            .Include(user => user.Provider)
            .FirstOrDefault(user =>
                user.Email == userMail
                && user.Provider.Name == "LDAP");

        if (user == null)
        {
            return null;
        }

        var userCredentialsValid = ldapAuth.UserCredentialsValid(userIdentifier, userPass);
        if (!userCredentialsValid)
        {
            return null;
        }

        return user;
    }

    public bool UserAuthentication(string userIdentifier, string userPass)
    {
        var decodedBase64Pass = Encoding.UTF8.GetString(Convert.FromBase64String(userPass));
        var userValid = false;
        try
        {
            userValid = ldapAuth.UserCredentialsValid(userIdentifier, decodedBase64Pass);
        }
        catch (Exception ex)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                Console.WriteLine($"UserLdapAuthentication-UserCredentialsValid: {ex}");
            }
        }

        return userValid;
    }


}
