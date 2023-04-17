using System;
using Unite.Identity.Data.Services;
using Unite.Identity.Data.Services.Configuration.Options;
using Unite.Identity.Services;
using Unite.Identity.Web.Configuration.Options;

namespace Unite.Identity.Web.Configuration.Extensions;

public static class ConfigurationExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddOptions();

        services.AddTransient<IdentityDbContext>();

        services.AddTransient<BaseIdentityService>();
        services.AddTransient<DefaultIdentityService>();
        services.AddTransient<LdapIdentityService>();
        services.AddTransient<SessionService>();
        services.AddTransient<UserService>();
    }

    private static void AddOptions(this IServiceCollection services)
    {
        services.AddTransient<ISqlOptions, SqlOptions>();
        services.AddTransient<ApiOptions>();
        services.AddTransient<RootOptions>();
    }
}

