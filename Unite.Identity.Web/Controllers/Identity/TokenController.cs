using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Web.Configuration.Options;
using Unite.Identity.Services;
using Unite.Identity.Web.Helpers;

namespace Unite.Identity.Web.Controllers.Identity;

[Route("api/token")]
public class TokenController : Controller
{
    private readonly ApiOptions _apiOptions;
    private readonly BaseIdentityService _baseIdentityService;
    private readonly SessionService _sessionService;
    private readonly ILogger _logger;

    public TokenController(
        ApiOptions apiOptions,
        BaseIdentityService baseIdentityService,
        SessionService sessionService,
        ILogger<TokenController> logger)
    {
        _apiOptions = apiOptions;
        _baseIdentityService = baseIdentityService;
        _sessionService = sessionService;
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Post(string login)
    {
        var session = CookieHelper.GetSessionCookie(Request);

        if (session == null)
        {
            _logger.LogWarning("Invalid attempt to refresh authorization token");

            return Unauthorized();
        }

        var user = _baseIdentityService.GetUser(login);

        if (user == null)
        {
            _logger.LogWarning("Invalid attempt to refresh authorization token for not existing user");

            return BadRequest();
        }

        var userSession = _sessionService.FindSession(user, session);

        if (userSession == null)
        {
            _logger.LogWarning("Invalid attempt to refresh authorization token for not existing session");

            return BadRequest();
        }

        var identity = ClaimsHelper.GetIdentity(user);

        var token = TokenHelper.GenerateAuthorizationToken(identity, _apiOptions.Key);

        CookieHelper.SetSessionCookie(Response, userSession.Session);

        return Ok(token);
    }
}
