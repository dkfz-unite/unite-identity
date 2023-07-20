using Unite.Identity.Constants;
using Unite.Identity.Data.Entities;
using Unite.Identity.Services;
using Unite.Identity.Web.Configuration.Options;

namespace Unite.Identity.Web.HostedServices;

public class RootHostedService : BackgroundService
{
    private readonly AdminOptions _adminOptions;
    private readonly DefaultProviderOptions _defaultProviderOptions;
    private readonly LdapProviderOptions _ldapProviderOptions;
    private readonly UserService _userService;
    private readonly ProviderService _providerService;
    private readonly AccountService _accountService;
    private readonly ILogger _logger;


    public RootHostedService(
        AdminOptions adminOptions,
        DefaultProviderOptions providerOptions,
        LdapProviderOptions ldapOptions,
        UserService userService,
        ProviderService providerService,
        AccountService accountService,
        ILogger<RootHostedService> logger)
    {
        _adminOptions = adminOptions;
        _defaultProviderOptions = providerOptions;
        _ldapProviderOptions = ldapOptions;
        _userService = userService;
        _providerService = providerService;
        _accountService = accountService;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Identity root service started");

        cancellationToken.Register(() => _logger.LogInformation("Identity root service stopped"));

        // Delay 5 seconds to let the web api start working
        await Task.Delay(5000, cancellationToken);
        
        
        var defaultProvider = ConfigureDefaultIdentityProvider(_defaultProviderOptions);

        var ldapProvider = ConfigureLdapIdentityProvider(_ldapProviderOptions);

        CreateRootAccount(defaultProvider.Id);
    }

    private Provider ConfigureDefaultIdentityProvider(DefaultProviderOptions options)
    {
        _logger.LogInformation("Configuring 'Default' identity provider");

        var provider = _providerService.GetProvider(provider => provider.Name == Providers.Default);

        if (provider == null && options.Active)
        {
            provider = _providerService.Add(
                Providers.Default, 
                options.Label, 
                options.Active, 
                options.Priority
            );
        }
        else if (provider != null)
        {
            provider = _providerService.Update(
                provider.Id,
                Providers.Default,
                options.Label,
                options.Active,
                options.Priority
            );
        }

        return provider;
    }

    private Provider ConfigureLdapIdentityProvider(LdapProviderOptions options)
    {
        _logger.LogInformation("Configuring 'LDAP' identity provider");

        var provider = _providerService.GetProvider(provider => provider.Name == Providers.Ldap);

        if (provider == null && options.Active)
        {
            provider = _providerService.Add(
                Providers.Ldap,
                options.Label,
                options.Active,
                options.Priority
            );
        }
        else if (provider != null)
        {
            provider = _providerService.Update(
                provider.Id,
                Providers.Ldap,
                options.Label,
                options.Active,
                options.Priority
            );
        }

        return provider;
    }

    private void CreateRootAccount(int providerId)
    {
        var user = _userService.GetUser(user => user.ProviderId == providerId && user.Email == _adminOptions.Login);

        if (user == null)
        {
            _logger.LogInformation("Configuring 'Root' user");

            _userService.Add(_adminOptions.Login, providerId, false, true, Permissions.RootPermissions);

            _accountService.CreateAccount(_adminOptions.Login, _adminOptions.Password);
        }
    }
}
