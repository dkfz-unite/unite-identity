using Unite.Identity.Constants;
using Unite.Identity.Data.Entities;
using Unite.Identity.Services;
using Unite.Identity.Web.Configuration.Options;

namespace Unite.Identity.Web.HostedServices;

public class RootHostedService : BackgroundService
{
    private readonly RootOptions _options;
    private readonly UserService _userService;
    private readonly DefaultIdentityService _defaultIdentityService;
    private readonly ProviderService _providerService;
    private readonly ILogger _logger;


    public RootHostedService(
        RootOptions options,
        UserService userService,
        DefaultIdentityService defaultIdentityService,
        ProviderService providerService,
        ILogger<RootHostedService> logger)
    {
        _options = options;
        _userService = userService;
        _defaultIdentityService = defaultIdentityService;
        _providerService = providerService;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Identity root service started");
        cancellationToken.Register(() => _logger.LogInformation("Identity root service stopped"));

        // Delay 5 seconds to let the web api start working
        //await Task.Delay(5000, cancellationToken);


        var defaultProvider = CreateDefaultProvider();

        CreateRootUser(defaultProvider);

        if (_options.UniteLdapActive == "true")
        {
            CreateUniteLdapProvider();
        }
    }

    private Provider CreateDefaultProvider()
    {
        var provider = _providerService.GetProvider(provider => provider.Name == _options.ProviderName);

        if (provider == null)
        {
            _logger.LogInformation("Configuring 'Root' provider");

            provider = _providerService.Add(_options.ProviderName, _options.ProviderName, true, Int32.Parse(_options.ProviderPriority));
        }

        return provider;
    }

    private void CreateRootUser(Provider defaultProvider)
    {
        var user = _defaultIdentityService.GetUser(_options.UserLogin);

        if (user == null)
        {
            _logger.LogInformation("Configuring 'Root' user");

            _userService.Add(_options.UserLogin, defaultProvider.Id, Permissions.RootPermissions);
            _defaultIdentityService.RegisterUser(_options.UserLogin, _options.UserPassword, true);
        }
    }

    private void CreateUniteLdapProvider()
    {
        var provider = _providerService.GetProvider(provider => provider.Name == "UniteLdap");

        if (provider == null)
        {
            _logger.LogInformation("Configuring 'UniteLdap' provider");

            provider = _providerService.Add("UniteLdap", "UniteLdap", true, 1);
        }
    }
}