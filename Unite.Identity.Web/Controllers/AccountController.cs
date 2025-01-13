using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unite.Identity.Web.Resources;
using Unite.Identity.Services;
using Unite.Identity.Web.Helpers;
using Unite.Identity.Web.Models;
using Unite.Identity.Web.Configuration.Constants;

namespace Unite.Identity.Web.Controllers;

[Route("api/account")]
[Authorize(Policy = Policies.User)]
public class AccountController: Controller
{
    private readonly AccountService _accountService;
    private readonly ILogger _logger;


    public AccountController(
        AccountService accountService, 
        ILogger<AccountController> logger)
    {
        _accountService = accountService;
        _logger = logger;
    }


    [HttpGet("")]
    public IActionResult GetAccount()
    {
        var provider = ClaimsHelper.GetValue(User.Claims, ClaimTypes.AuthenticationMethod);

        var email = ClaimsHelper.GetValue(User.Claims, ClaimTypes.Email);

        var user = _accountService.GetAccount(email, provider);

        var account = new AccountResource(user);

        return Json(account);
    }

    [HttpPost("")]
    [AllowAnonymous]
    public IActionResult CreateAccount([FromBody]CreateAccountModel model)
    {
        var user = _accountService.CreateAccount(model.Email, model.Password);

        if (user == null)
        {
            return BadRequest("Email address is not in access list or already registered");
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

        var deleted = _accountService.DeleteAccount(email, provider);

        if (deleted == false)
        {
            return NotFound();
        }

        return Ok(email);
    }

    [HttpPut("password")]
    public IActionResult ChangePassword([FromBody]ChangePasswordModel model)
    {
        var email = ClaimsHelper.GetValue(User.Claims, ClaimTypes.Email);

        var user = _accountService.ChangePassword(email, model.NewPassword);

        if (user == null)
        {
            return BadRequest("Invalid old password");
        }

        return Ok();
    }
}
