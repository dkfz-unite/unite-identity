using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unite.Identity.Web.Resources;
using Unite.Identity.Services;
using Unite.Identity.Web.Helpers;
using Unite.Identity.Web.Models;

namespace Unite.Identity.Web.Controllers;

[Route("api/account")]
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
    [Authorize]
    public IActionResult GetAccount()
    {
        var provider = ClaimsHelper.GetValue(User.Claims, ClaimTypes.AuthenticationMethod);

        var email = ClaimsHelper.GetValue(User.Claims, ClaimTypes.Email);

        var user = _accountService.GetAccount(email, provider);

        var account = new AccountResource(user);

        return Json(account);
    }

    [HttpPost("")]
    public IActionResult CreateAccount([FromBody]CreateAccountModel model)
    {
        var user = _accountService.CreateAccount(model.Email, model.Password);

        if (user == null)
        {
            return BadRequest("Email address is not in access list or already registered");
        }

        return Ok();
    }

    [HttpPut("password")]
    [Authorize]
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
