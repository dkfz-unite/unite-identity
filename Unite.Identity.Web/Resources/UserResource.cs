using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Extensions;

namespace Unite.Identity.Web.Resources;

public class UserResource
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Provider { get; set; }
    public string[] Permissions { get; set; }
    

    public UserResource(User user)
    {
        Id = user.Id;
        Email = user.Email;
        Provider = user.Provider.Label;

        if (user.UserPermissions?.Any() == true)
        {
            Permissions = user.UserPermissions
                .Select(userPermission => userPermission.PermissionId.ToDefinitionString())
                .ToArray();
        }
    }
}
