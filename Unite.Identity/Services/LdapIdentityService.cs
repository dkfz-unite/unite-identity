using System;
using System.Text;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Services;
using Unite.Identity.Shared;

namespace Unite.Identity.Services;

public class LdapIdentityService : BaseIdentityService, IIdentityService
{
    public LdapIdentityService(IdentityDbContext dbContext) : base(dbContext)
    {
    }

    public User LoginUser(string userIdentifier, string userPass)
    {
        throw new NotImplementedException();
    }

    public bool UserAuthentication(string userIdentifier, string userPass)
    {
        var targetOU = "ToDo"; // Environment.GetEnvironmentVariable("LDAP_USER_TARGET_OU");
        var ldapServer = "ToDo"; // Environment.GetEnvironmentVariable("LDAP_SERVER");
        var portNumber = "ToDo"; // Environment.GetEnvironmentVariable("LDAP_PORT");
        var serviceUserRNA = "ToDo"; // Environment.GetEnvironmentVariable("LDAP_SERVICE_USER_RNA");
        var serviceUserPassword = "ToDo"; // Environment.GetEnvironmentVariable("LDAP_SERVICE_USER_PASSWORD");

        if (ldapServer == null || serviceUserRNA == null || serviceUserPassword == null || targetOU == null)
        {
            throw new ArgumentException("Missing environment variables! Configuration incomplete!!");
        }

        int? finalPortNumber = null;
        if (portNumber != null)
        {
            finalPortNumber = Int16.Parse(portNumber);
        }

        var ldapAuthenticator = new LDAPAuthentication(ldapServer, serviceUserRNA, serviceUserPassword, targetOU, finalPortNumber);

        var decodedBase64Pass = Encoding.UTF8.GetString(Convert.FromBase64String(userPass));
        var userValid = false;
        try
        {
            userValid = ldapAuthenticator.UserCredentialsValid(userIdentifier, decodedBase64Pass);
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
