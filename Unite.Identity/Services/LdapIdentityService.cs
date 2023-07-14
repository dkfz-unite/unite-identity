﻿using Unite.Identity.Constants;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Services;
using Unite.Identity.Data.Services.Configuration.Options;
using Unite.Identity.Shared;

namespace Unite.Identity.Services;

public class LdapIdentityService : BaseIdentityService, IIdentityService
{
    private readonly LDAPAuthentication _ldapAuth;

    public LdapIdentityService(
        IdentityDbContext dbContext,
        ILdapOptions options) : base(dbContext)
    {
        _ldapAuth = new LDAPAuthentication(options.Server, options.ServiceUserRNA, options.ServiceUserPassword, options.UserTargetOU, options.Port);
    }

    public User LoginUser(string userIdentifier, string userPass)
    {
        string userEmail = userIdentifier.Contains("@")
            ? userIdentifier
            : GetUserEmail(userIdentifier);
        if (userEmail == null)
        {
            return null;
        }

        var user = GetUser(userEmail, Providers.Ldap);
        if (user == null)
        {
            return null;
        }

        var userCredentialsValid = _ldapAuth.UserCredentialsValid(userIdentifier, userPass);
        if (!userCredentialsValid)
        {
            return null;
        }

        return user;
    }

    private string GetUserEmail(string userIdentifier)
    {
        string userEmail = null;
        var result = _ldapAuth.ReadUserEntry(userIdentifier);
        if (result != null)
        {
            try
            {
                userEmail = result.Attributes["mail"][0].ToString();
            }
            catch (Exception ex) { }
        }

        return userEmail;
    }
}
