using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Services;
using Unite.Identity.Configuration.Options;
using Unite.Identity.Helpers;
using Unite.Identity.Models;

namespace Unite.Identity.Controllers;

[Route("api/identity/[controller]")]
public class SignInController : Controller
{
    private readonly ApiOptions _apiOptions;
    private readonly IdentityService _identityService;
    private readonly SessionService _sessionService;
    private readonly ILogger _logger;


    public SignInController(
        ApiOptions apiOptions,
        IdentityService identityService,
        SessionService sessionService,
        ILogger<SignInController> logger)
    {
        _apiOptions = apiOptions;
        _identityService = identityService;
        _sessionService = sessionService;
        _logger = logger;
    }


    public IActionResult Post([FromBody] SignInModel model, [FromHeader(Name = "User-Agent")] string client)
    {
        var user = _identityService.SignInUser(model.Email, model.Password);

        if (user == null)
        {
            var invalidCredentialsErrorMessage = $"Invalid login or password";

            _logger.LogWarning($"{invalidCredentialsErrorMessage} for user '{model.Email}'");

            return BadRequest(invalidCredentialsErrorMessage);
        }

        var userSession = _sessionService.CreateSession(user, client);

        var identity = ClaimsHelper.GetIdentity(user);

        var token = TokenHelper.GenerateAuthorizationToken(identity, _apiOptions.Key);

        CookieHelper.SetSessionCookie(Response, userSession.Session);

        return Ok(token);
    }
}
