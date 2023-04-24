using Unite.Identity.Constants;
using Unite.Identity.Services;
using Unite.Identity.Web.Configuration.Options;

namespace Unite.Identity.Web.HostedServices;

public class RootHostedService : BackgroundService
{
    private readonly RootOptions _options;
    private readonly UserService _userService;
    private readonly DefaultIdentityService _defaultIdentityService;
    private readonly ILogger _logger;


    public RootHostedService(
        RootOptions options,
        UserService userService,
        DefaultIdentityService defaultIdentityService,
        ILogger<RootHostedService> logger)
    {
        _options = options;
        _userService = userService;
        _defaultIdentityService = defaultIdentityService;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        // Delay 5 seconds to let the web api start working
        await Task.Delay(5000, cancellationToken);

        var user = _defaultIdentityService.GetUser(_options.User);

        if (user == null)
        {
            _logger.LogInformation("Configuring 'Root' user");

            _userService.Add(_options.User, Permissions.RootPermissions);
            _defaultIdentityService.RegisterUser(_options.User, _options.Password, true);
        }
    }
}