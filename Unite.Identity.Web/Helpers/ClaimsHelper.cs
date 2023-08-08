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

    public static ClaimsIdentity GetIdentity(Worker worker)
    {
        var claims = new List<Claim>();

        claims.Add(new Claim(ClaimTypes.Actor, Actors.Worker));
        claims.Add(new Claim(ClaimTypes.Name, worker.Name));

        if (worker.WorkerPermissions != null)
        {
            foreach (var workerPermission in worker.WorkerPermissions)
            {
                claims.Add(new Claim(PermissionClaimType, workerPermission.PermissionId.ToDefinitionString()));
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
