using Unite.Identity.Data.Entities;

namespace Unite.Identity.Services;

public interface IIdentityService
{
    //public bool UserAuthentication(string userIdentifier, string userPass);

    public User GetUser(string email);

    public User SignInUser(string email, string password);
}
