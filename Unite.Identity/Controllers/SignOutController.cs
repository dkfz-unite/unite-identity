using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unite.Identity.Helpers;
using Unite.Identity.Services;

namespace Unite.Identity.Controllers;

[Route("api/identity/[controller]")]
[Authorize]
public class SignOutController : Controller
{
    private readonly IdentityService _identityService;
    private readonly SessionService _sessionService;
    private readonly ILogger _logger;

    public SignOutController(
        IdentityService identityService,
        SessionService sessionService,
        ILogger<SignOutController> logger)
    {
        _identityService = identityService;
        _sessionService = sessionService;
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Post()
    {
        var session = CookieHelper.GetSessionCookie(Request);

        if (session != null)
        {
            var email = ClaimsHelper.GetValue(User.Claims, ClaimTypes.Email);

            var user = _identityService.GetUser(email);

            if (user == null)
            {
                _logger.LogWarning("Invalid attempt to sign out not existing user");

                return BadRequest();
            }

            var userSession = _sessionService.FindSession(user, session);

            if (userSession == null)
            {
                _logger.LogWarning("Invalid attempt to remove not existing session");

                return BadRequest();
            }

            _sessionService.RemoveSession(userSession);

            CookieHelper.DeleteSessionCookie(Response);
        }

        return Ok();
    }
}
