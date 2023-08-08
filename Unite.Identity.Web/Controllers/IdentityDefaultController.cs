using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Constants;
using Unite.Identity.Services;
using Unite.Identity.Web.Configuration.Options;

namespace Unite.Identity.Web.Controllers;

[Route("api/realm/default")]
public class IdentityDefaultController : IdentityController<DefaultIdentityService>
{
    protected override string Provider => Providers.Default;

    public IdentityDefaultController(
        ApiOptions apiOptions,
        UserService userService,
        ProviderService providerService,
        SessionService sessionService,
        DefaultIdentityService identityService,
        ILogger<IdentityDefaultController> logger)
        : base(apiOptions, userService, providerService, sessionService, identityService, logger) { }
}
