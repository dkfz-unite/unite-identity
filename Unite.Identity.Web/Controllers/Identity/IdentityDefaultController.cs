using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unite.Identity.Web.Configuration.Options;
using Unite.Identity.Models;
using Unite.Identity.Services;
using Unite.Identity.Web.Helpers;
using Unite.Identity.Data.Entities;

namespace Unite.Identity.Web.Controllers.Identity;

[Route("api/default")]
public class IdentityDefaultController : ControllerBase
{
    private readonly ApiOptions _apiOptions;
    private readonly DefaultIdentityService _defaultIdentityService;
    private readonly SessionService _sessionService;
    private readonly ILogger _logger;

    public IdentityDefaultController(
        ApiOptions apiOptions,
        DefaultIdentityService defaultIdentityService,
        SessionService sessionService,
        ILogger<IdentityDefaultController> logger)
    {
        _apiOptions = apiOptions;
        _defaultIdentityService = defaultIdentityService;
        _sessionService = sessionService;
        _logger = logger;
    }

    //(string userId, string userPass)

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterModel registerModel)
    {
        var user = _defaultIdentityService.RegisterUser(registerModel.Email, registerModel.Password);

        return user != null ? Ok() : BadRequest($"Email address '{registerModel.Email}' is not in access list or already registered");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel loginModel, [FromHeader(Name = "User-Agent")] string client)
    {
        var user = _defaultIdentityService.LoginUser(loginModel.Email, loginModel.Password);

        if (user == null)
        {
            var invalidCredentialsErrorMessage = $"Invalid login or password";

            _logger.LogWarning($"{invalidCredentialsErrorMessage} for user '{loginModel.Email}'");

            return BadRequest(invalidCredentialsErrorMessage);
        }

        var userSession = _sessionService.CreateSession(user, client);

        var identity = ClaimsHelper.GetIdentity(user);

        var token = TokenHelper.GenerateAuthorizationToken(identity, _apiOptions.Key);

        CookieHelper.SetSessionCookie(Response, userSession.Session);

        return Ok(token);
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var session = CookieHelper.GetSessionCookie(Request);

        if (session != null)
        {
            var email = ClaimsHelper.GetValue(User.Claims, ClaimTypes.Email);

            var user = _defaultIdentityService.GetUser(email);

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

    [Authorize]
    [HttpPut("change-password")]
    public IActionResult Put([FromBody] PasswordChangeModel model)
    {
        var email = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;

        var currentUser = _defaultIdentityService.GetUser(email);

        var updatedUser = _defaultIdentityService.ChangePassword(currentUser.Email, model.OldPassword, model.NewPassword);

        if (updatedUser == null)
        {
            return BadRequest($"Invalid old password");
        }

        return Ok();
    }
}
