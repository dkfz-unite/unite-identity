using System;
namespace Unite.Identity.Interfaces;

public interface IAuthenticationInterface
{
    public bool UserAuthentication(string userIdentifier, string userPass);
}
