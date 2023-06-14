using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Extensions;

namespace Unite.Identity.Models;

public class UserResource
{
    public int Id { get; set; }
    public string Email { get; set; }
    public bool IsActive { get; set; }
    public string[] Permissions { get; set; }

    public UserResource(User user)
    {
        Id = user.Id;
        Email = user.Email;
        IsActive = user.IsActive;

        if (user.UserPermissions != null && user.UserPermissions.Any())
        {
            Permissions = user.UserPermissions
                .Select(userPermission => userPermission.PermissionId.ToDefinitionString())
                .ToArray();
        }
    }
}