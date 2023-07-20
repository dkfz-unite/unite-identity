using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using Unite.Identity.Web.Helpers;
using Unite.Identity.Services;
using Unite.Identity.Web.Configuration.Options;
using Unite.Identity.Web.Resources;

namespace Unite.Identity.Web.Controllers;

[Route("api/users")]
[Authorize(Roles = "Root")]
public class UsersController : Controller
{
    private readonly UserService _userService;
    private readonly AdminOptions _rootOptions;


    public UsersController(
        UserService userService,
        AdminOptions rootOptions)
    {
        _userService = userService;
        _rootOptions = rootOptions;
    }

    [HttpGet("")]
    public IActionResult Get()
    {
        var currentUserEmail = ClaimsHelper.GetValue(User.Claims, ClaimTypes.Email);
        var rootUserEmail = _rootOptions.Login;

        var users = _userService
            .GetUsers(user => user.Email != currentUserEmail && user.Email != rootUserEmail)
            .Select(user => new UserResource(user))
            .ToArray();

        return Json(users);
    }
}

