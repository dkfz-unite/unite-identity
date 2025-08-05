using System.Security.Claims;
using Unite.Identity.Data.Entities;
using Unite.Identity.Data.Extensions;
using Unite.Identity.Web.Configuration.Constants;

namespace Unite.Identity.Web.Helpers;

public class ClaimsHelper
{
    public const string PermissionClaimType = "permission";

    public static ClaimsIdentity GetIdentity(User user)
    {
        var claims = new List<Claim>();

        claims.Add(new Claim(ClaimTypes.Actor, Actors.User));
        claims.Add(new Claim(ClaimTypes.Email, user.Email));
        claims.Add(new Claim(ClaimTypes.AuthenticationMethod, user.Provider.Name));

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

    public static ClaimsIdentity GetIdentity(Token token)
    {
        var claims = new List<Claim>();

        claims.Add(new Claim(ClaimTypes.Actor, Actors.Worker));
        claims.Add(new Claim(ClaimTypes.Name, token.Name));
        claims.Add(new Claim(ClaimTypes.Sid, $"{token.Key}"));

        if (token.TokenPermissions != null)
        {
            foreach (var tokenPermission in token.TokenPermissions)
            {
                claims.Add(new Claim(PermissionClaimType, tokenPermission.PermissionId.ToDefinitionString()));
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
