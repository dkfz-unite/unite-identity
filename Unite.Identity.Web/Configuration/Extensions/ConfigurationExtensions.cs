using System;
using FluentValidation;
using FluentValidation.AspNetCore;
using Unite.Identity.Data.Services;
using Unite.Identity.Data.Services.Configuration.Options;
using Unite.Identity.Models;
using Unite.Identity.Models.Validators;
using Unite.Identity.Services;
using Unite.Identity.Web.Configuration.Options;
using Unite.Identity.Web.HostedServices;

namespace Unite.Identity.Web.Configuration.Extensions;

public static class ConfigurationExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddOptions();
        services.AddValidation();

        services.AddTransient<IdentityDbContext>();

        services.AddTransient<BaseIdentityService>();
        services.AddTransient<DefaultIdentityService>();
        services.AddTransient<LdapIdentityService>();
        services.AddTransient<SessionService>();
        services.AddTransient<UserService>();
        services.AddTransient<ProviderService>();

        services.AddHostedService<RootHostedService>();
    }

    private static void AddOptions(this IServiceCollection services)
    {
        services.AddTransient<ISqlOptions, SqlOptions>();
        services.AddTransient<ApiOptions>();
        services.AddTransient<RootOptions>();
    }

    private static void AddValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();

        services.AddTransient<IValidator<AddUserModel>, AddUserModelValidator>();
        services.AddTransient<IValidator<EditUserModel>, EditUserModelValidator>();
        services.AddTransient<IValidator<RegisterModel>, RegisterModelValidator>();
        services.AddTransient<IValidator<LoginModel>, LoginModelValidator>();
        services.AddTransient<IValidator<PasswordChangeModel>, PasswordChangeModelValidator>();
        services.AddTransient<IValidator<AddProviderModel>, AddProviderModelValidator>();
        services.AddTransient<IValidator<EditProviderModel>, EditProviderModelValidator>();
    }
}
