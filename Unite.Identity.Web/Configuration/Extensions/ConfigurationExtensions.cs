using FluentValidation;
using FluentValidation.AspNetCore;
using Unite.Cache.Configuration.Options;
using Unite.Data.Context;
using Unite.Identity.Data.Services;
using Unite.Identity.Data.Services.Configuration.Options;
using Unite.Identity.Services;
using Unite.Identity.Services.Ldap;
using Unite.Identity.Services.Ldap.Configuration.Options;
using Unite.Identity.Web.Configuration.Options;
using Unite.Identity.Web.Models;
using Unite.Identity.Web.Models.Validators;
using Unite.Identity.Web.Workers;

namespace Unite.Identity.Web.Configuration.Extensions;

public static class ConfigurationExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddOptions();
        services.AddValidation();

        services.AddTransient<IdentityDbContext>();
        services.AddTransient<DomainDbContext>();

        services.AddTransient<UserService>();
        services.AddTransient<UserDataService>();
        services.AddTransient<ProviderService>();
        services.AddTransient<SessionService>();
        services.AddTransient<TokenService>();
        services.AddTransient<LdapService>();
        services.AddTransient<LdapIdentityService>();
        services.AddTransient<DefaultIdentityService>();
        services.AddTransient<AccountService>();
        
        services.AddHostedService<RootWorker>();
        services.AddHostedService<AccountWorker>();
    }

    private static void AddOptions(this IServiceCollection services)
    {
        services.AddTransient<ISqlOptions, SqlOptions>();
        services.AddTransient<IMongoOptions, MongoOptions>();
        services.AddTransient<ApiOptions>();
        services.AddTransient<AdminOptions>();
        services.AddTransient<DefaultProviderOptions>();
        services.AddTransient<LdapProviderOptions>();
        services.AddTransient<ILdapOptions, LdapProviderOptions>();        
    }

    private static void AddValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();

        services.AddTransient<IValidator<AddUserModel>, AddUserModelValidator>();
        services.AddTransient<IValidator<EditUserModel>, EditUserModelValidator>();
        services.AddTransient<IValidator<AddProviderModel>, AddProviderModelValidator>();
        services.AddTransient<IValidator<EditProviderModel>, EditProviderModelValidator>();
        services.AddTransient<IValidator<AddTokenModel>, AddTokenModelValidator>();
        services.AddTransient<IValidator<EditTokenModel>, EditTokenModelValidator>();
        services.AddTransient<IValidator<EpiryDateModel>, ExpiryDateModelValidator>();
        services.AddTransient<IValidator<IdentityModel>, IdentityModelValidator>();
        services.AddTransient<IValidator<CreateAccountModel>, CreateAccountModelValidator>();
        services.AddTransient<IValidator<ChangePasswordModel>, ChangePasswordModelValidator>();
    }
}
