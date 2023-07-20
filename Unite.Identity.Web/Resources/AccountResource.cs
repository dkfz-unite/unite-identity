using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Extensions;

namespace Unite.Identity.Web.Resources;

public class AccountResource
{
    public string Email { get; set; }
    public string Provider { get; set; }
    public string[] Permissions { get; set; }
    public string[] Devices { get; set; }


    public AccountResource(User user)
    {
        Email = user.Email;

        Provider = user.Provider.Label ?? user.Provider.Name;

        Permissions = user.UserPermissions?
            .Select(userPermission => userPermission.PermissionId.ToDefinitionString())
            .ToArray();

        Devices = user.UserSessions?
            .Select(session => session.Client)
            .ToArray();
    }
}
