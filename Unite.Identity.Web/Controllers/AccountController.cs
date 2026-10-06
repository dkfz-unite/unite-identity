using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unite.Identity.Web.Resources;
using Unite.Identity.Services;
using Unite.Identity.Web.Helpers;
using Unite.Identity.Web.Models;
using Unite.Identity.Web.Configuration.Constants;
using Unite.Identity.Web.Configuration.Extensions;
using Unite.Identity.Web.Configuration.Options;
using Unite.Post;
using Unite.Post.Configuration.Options;

namespace Unite.Identity.Web.Controllers;

[Route("api/account")]
[Authorize(Policy = Policies.User)]
public class AccountController: Controller
{
    private readonly AccountService _accountService;
    private readonly MailService _mailService;
    private readonly InstanceOptions _instanceOptions;
    private readonly ISmtpOptions _smtpOptions;
    private readonly ILogger _logger;


    public AccountController(
        AccountService accountService,
        MailService mailService,
        InstanceOptions instanceOptions,
        ISmtpOptions smtpOptions,
        ILogger<AccountController> logger)
    {
        _accountService = accountService;
        _mailService = mailService;
        _instanceOptions = instanceOptions;
        _smtpOptions = smtpOptions;
        _logger = logger;
    }


    [HttpGet("")]
    public IActionResult GetAccount()
    {
        var provider = ClaimsHelper.GetValue(User.Claims, ClaimTypes.AuthenticationMethod);

        var email = ClaimsHelper.GetValue(User.Claims, ClaimTypes.Email);

        var user = _accountService.Get(email, provider);

        var account = new AccountResource(user);

        return Json(account);
    }

    [HttpPost("")]
    [AllowAnonymous]
    public IActionResult CreateAccount([FromBody]CreateAccountModel model)
    {
        if (_instanceOptions.Public)
        {
            var user = _accountService.AddPublic(model.Email, model.Password);

            if (user == null)
            {
                return BadRequest("Email address is already registered");
            }
        }
        else
        {
            var user = _accountService.AddPrivate(model.Email, model.Password);

            if (user == null)
            {
                return BadRequest("Email address is not in access list or already registered");
            }
        }

        return Ok();
    }

    [HttpDelete("")]
    public IActionResult DeleteAccount()
    {
        var role = ClaimsHelper.GetValue(User.Claims, ClaimTypes.Role);

        if (role != null && role.Contains("Root"))
        {
            return BadRequest("Root account cannot be deleted");
        }

        var provider = ClaimsHelper.GetValue(User.Claims, ClaimTypes.AuthenticationMethod);

        var email = ClaimsHelper.GetValue(User.Claims, ClaimTypes.Email);

        var deleted = _accountService.Delete(email, provider);

        if (deleted == false)
        {
            return NotFound();
        }

        CookieHelper.DeleteSessionCookie(Response);
        
        return Ok();
    }

    [HttpPut("password")]
    public IActionResult ChangePassword([FromBody]ChangePasswordModel model)
    {
        var email = ClaimsHelper.GetValue(User.Claims, ClaimTypes.Email);

        var user = _accountService.ChangePassword(email, model.NewPassword, model.OldPassword);

        if (user == null)
        {
            return BadRequest("Invalid old password");
        }

        return Ok();
    }

    [HttpPost("password-reset")]
    [AllowAnonymous]
    public IActionResult RequestPasswordReset([FromBody]ResetPasswordRequestModel model)
    {
        if (!IsSmtpConfigured(out var result))
        {
            return result;
        }

        var token = _accountService.RequestPasswordReset(model.Email, _instanceOptions.ResetTokenLifetime);

        if (token != null)
        {
            var data = new Post.Mails.PasswordReset { Host = _instanceOptions.Host, Token = token };
            
            _mailService.SendPasswordResetMail(model.Email, data);
        }
        else
        {
            _logger.LogWarning("Password reset requested for non-existent email: {Email}", model.Email);
        }

        return Ok("Password reset link has been sent if the email exists");
    }

    [HttpPost("password-reset-confirm")]
    [AllowAnonymous]
    public IActionResult ConfirmPasswordReset([FromBody]ResetPasswordConfirmationModel model)
    {
        if (!IsSmtpConfigured(out var result))
        {
            return result;
        }

        var user = _accountService.ConfirmPasswordReset(model.Token, model.Password);

        if (user != null)
        {
            return Ok("Password has been successfully reset");
        }
        else
        {
            return BadRequest("Could not reset password");
        }
    }


    private bool IsSmtpConfigured(out IActionResult result)
    {
        result = null;

        if (_smtpOptions.IsConfigured())
        {
            return true;
        }

        _logger.LogWarning("Password reset attempted, but SMTP is not configured");

        result = StatusCode(StatusCodes.Status503ServiceUnavailable,
            "Password reset is unavailable on this instance.");

        return false;
    }
}
