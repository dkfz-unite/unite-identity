using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Data.Entities;
using Unite.Identity.Services;
using Unite.Identity.Web.Configuration.Options;
using Unite.Identity.Web.Helpers;
using Unite.Identity.Web.Models;

namespace Unite.Identity.Web.Controllers;

public abstract class IdentityController<TIdentityService> : Controller where TIdentityService : IIdentityService
{
    protected readonly ApiOptions _apiOptions;
    protected readonly UserService _userService;
    protected readonly ProviderService _providerService;
    protected readonly SessionService _sessionService;
    protected readonly TIdentityService _identityService;
    protected readonly ILogger _logger;

    protected abstract string Provider { get; }
    
    
    protected IdentityController(
        ApiOptions apiOptions,
        UserService userService, 
        ProviderService providerService,
        SessionService sessionService,
        TIdentityService identityService,
        ILogger logger)
    {
        _apiOptions = apiOptions;
        _userService = userService;
        _providerService = providerService;
        _sessionService = sessionService;
        _identityService = identityService;
        _logger = logger;
    }
    

    [HttpPost("login")]
    public virtual IActionResult Login([FromBody]IdentityModel model, string client)
    {
        var user = _identityService.LoginUser(model.Email, model.Password);

        if (user == null)
        {
            var invalidCredentialsErrorMessage = $"Invalid login or password";

            _logger.LogWarning(invalidCredentialsErrorMessage);

            return BadRequest(invalidCredentialsErrorMessage);
        }

        var identity = ClaimsHelper.GetIdentity(user);

        var userSession = _sessionService.CreateSession(user, client);

        CookieHelper.SetSessionCookie(Response, userSession.Session);

        var token = TokenHelper.GenerateAuthorizationToken(identity, _apiOptions.Key);

        return Ok(token);
    }

    [HttpPost("logout")]
    [Authorize]
    public virtual IActionResult Logout()
    {
        var email = ClaimsHelper.GetValue(User.Claims, ClaimTypes.Email);

        var user = FindUser(email);

        if (user == null)
        {
            _logger.LogWarning("Invalid attempt to sign out not existing user");

            return BadRequest();
        }

        var session = CookieHelper.GetSessionCookie(Request);
    
        if (session != null)
        {
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

    [HttpPost("token")]
    public virtual IActionResult RefreshToken([FromQuery]string email)
    {
        var session = CookieHelper.GetSessionCookie(Request);

        if (session == null)
        {
            _logger.LogWarning("Invalid attempt to refresh authorization token");

            return Unauthorized();
        }

        var user = FindUser(email);

        if (user == null)
        {
            _logger.LogWarning("Invalid attempt to get authorization token for not existing user");

            return BadRequest();
        }

        var identity = ClaimsHelper.GetIdentity(user);

        var token = TokenHelper.GenerateAuthorizationToken(identity, _apiOptions.Key);

        return Ok(token);
    }


    protected User FindUser(string email, bool isActive = true)
    {
        return _userService.GetUser(user =>
            user.Provider.Name == Provider && 
            user.Email == email && 
            user.IsActive == isActive
        );
    }
}
