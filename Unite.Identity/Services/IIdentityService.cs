using Unite.Identity.Data.Entities;

namespace Unite.Identity.Services;

public interface IIdentityService
{
    User LoginUser(string login, string password);
}
