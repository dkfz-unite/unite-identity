using System;
using Unite.Identity.Interfaces;

namespace Unite.Identity.Services;

public class UniteAuthenticationService : IAuthenticationInterface
{
    public bool UserAuthentication(string userIdentifier, string userPass)
    {
        throw new NotImplementedException();
    }

    public UniteAuthenticationService()
    {
    }
}
