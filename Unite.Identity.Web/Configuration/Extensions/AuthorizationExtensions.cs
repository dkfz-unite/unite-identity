using System;
using Microsoft.AspNetCore.Authorization;

namespace Unite.Identity.Web.Configuration.Extensions
{
    public static class AuthorizationExtensions
    {
        public static void AddAuthorizationOptions(this AuthorizationOptions options)
        {
            options.AddPolicy("Data.Manager", policy => policy
                .RequireClaim("permission", "Data.Write")
                .RequireClaim("permission", "Data.Edit")
                .RequireClaim("permission", "Data.Delete")
            );
        }
    }
}

