using Unite.Identity.Data.Entities.Enums;

namespace Unite.Identity.Constants;

public static class Permissions
{
    public static readonly Permission[] DefaultPermissions =
    {
            Permission.DataRead
        };

    public static readonly Permission[] RootPermissions = Enum.GetValues<Permission>();
}
