using System.Security.Claims;
using Unite.Identity.Constants;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Extensions;

namespace Unite.Identity.Web.Helpers;

public class ClaimsHelper
{
    public const string PermissionClaimType = "permission";

    public static ClaimsIdentity GetIdentity(User user)
    {
        var claims = new List<Claim>();

        claims.Add(new Claim(ClaimTypes.Email, user.Email));

        if (string.Equals(user.Provider?.Name, Providers.Ldap))
        {
            claims.Add(new Claim(ClaimTypes.AuthenticationMethod, "ldap"));
        }
        else if (string.Equals(user.Provider?.Name, Providers.Default))
        {
            claims.Add(new Claim(ClaimTypes.AuthenticationMethod, "default"));
        }

        if (user.IsRoot)
        {
            claims.Add(new Claim(ClaimTypes.Role, "Root"));
        }

        if (user.UserPermissions != null)
        {
            foreach (var userPermission in user.UserPermissions)
            {
                claims.Add(new Claim(PermissionClaimType, userPermission.PermissionId.ToDefinitionString()));
            }
        }

        var identity = new ClaimsIdentity(claims);

        return identity;
    }

    public static string GetValue(IEnumerable<Claim> claims, string name)
    {
        return claims.FirstOrDefault(claim => claim.Type == name)?.Value;
    }
}
