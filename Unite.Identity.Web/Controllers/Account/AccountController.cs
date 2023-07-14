﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unite.Identity.Data.Entities;
using Unite.Identity.Resources;
using Unite.Identity.Services;
using Unite.Identity.Data.Extensions;

namespace Unite.Identity.Web.Controllers.Account;

[Route("api/account")]
[Authorize]
public class AccountController : Controller
{
    private readonly BaseIdentityService _baseIdentityService;


    public AccountController(
        BaseIdentityService identityService)
    {
        _baseIdentityService = identityService;
    }


    [HttpGet]
    public IActionResult Get()
    {
        var currentUser = GetCurrentUser();

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

        var user = _baseIdentityService.GetUser(email);

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

        account.Provider = user.Provider?.Label != null && user.Provider?.Label != ""
            ? user.Provider?.Label
            : user.Provider?.Name;

        return account;
    }
}
