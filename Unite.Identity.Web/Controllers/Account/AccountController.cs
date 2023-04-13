using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unite.Identity.Data.Entities;
using Unite.Identity.Models;
using Unite.Identity.Resources;
using Unite.Identity.Services;
using Unite.Identity.Data.Extensions;

namespace Unite.Identity.Web.Controllers.Account;

[Route("api/account")]
[Authorize]
public class AccountController : Controller
{
    private readonly DefaultIdentityService _defaultIdentityService;
    private readonly ILogger _logger;


    public AccountController(
        DefaultIdentityService identityService,
        ILogger<AccountController> logger)
    {
        _defaultIdentityService = identityService;
        _logger = logger;
    }


    [HttpGet]
    public IActionResult Get()
    {
        var currentUser = GetCurrentUser();

        var account = CreateFrom(currentUser);

        return Json(account);
    }

    [HttpPut]
    public IActionResult Put([FromBody] PasswordChangeModel model)
    {
        var currentUser = GetCurrentUser();

        var updatedUser = _defaultIdentityService.ChangePassword(currentUser.Email, model.OldPassword, model.NewPassword);

        if (updatedUser == null)
        {
            return BadRequest($"Invalid old password");
        }

        var account = CreateFrom(currentUser);

        return Json(account);
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        throw new NotImplementedException();
    }


    private User GetCurrentUser()
    {
        var email = HttpContext.User.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;

        var user = _defaultIdentityService.GetUser(email);

        return user;
    }

    private AccountResource CreateFrom(User user)
    {
        var account = new AccountResource();

        account.Email = user.Email;

        account.Devices = user.UserSessions?
            .Select(session => session.Client)
            .ToArray();

        account.Permissions = user.UserPermissions?
            .Select(userPermission => userPermission.PermissionId.ToDefinitionString())
            .ToArray();

        return account;
    }
}
