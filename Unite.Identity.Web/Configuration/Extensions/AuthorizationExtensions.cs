using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Unite.Identity.Web.Configuration.Constants;

namespace Unite.Identity.Web.Configuration.Extensions;

public static class AuthorizationExtensions
{
    public static void AddAuthorizationOptions(this AuthorizationOptions options)
    {
        options.AddPolicy(Policies.User, policy => policy
            .RequireClaim(ClaimTypes.Actor, Actors.User)
        );
    }
}
