using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Constants;
using Unite.Identity.Services;
using Unite.Identity.Web.Configuration.Options;

namespace Unite.Identity.Web.Controllers;

[Route("api/realm/ldap")]
public class IdentityLdapController : IdentityController<LdapIdentityService>
{
    protected override string Provider => Providers.Ldap;

    public IdentityLdapController(
        ApiOptions apiOptions, 
        UserService userService, 
        ProviderService providerService,
        SessionService sessionService,
        LdapIdentityService identityService,
        ILogger<IdentityLdapController> logger) 
        : base(apiOptions, userService, providerService, sessionService, identityService, logger) { }
}
