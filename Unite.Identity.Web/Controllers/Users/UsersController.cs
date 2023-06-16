using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using Unite.Identity.Web.Helpers;
using Unite.Identity.Models;
using Unite.Identity.Services;
using Unite.Identity.Web.Configuration.Options;

namespace Unite.Identity.Web.Controllers.Users;

[Route("api/users")]
[ApiController]
[Authorize(Roles = "Root")]
public class UsersController : Controller
{
    private readonly UserService _userService;
    private readonly RootOptions _rootOptions;


    public UsersController(
        UserService userService,
        RootOptions rootOptions)
    {
        _userService = userService;
        _rootOptions = rootOptions;
    }

    [HttpGet("")]
    public IActionResult Get()
    {
        var currentUserEmail = ClaimsHelper.GetValue(User.Claims, ClaimTypes.Email);
        var rootUserEmail = _rootOptions.UserLogin;

        var users = _userService
            .GetUsers(user => user.Email != currentUserEmail && user.Email != rootUserEmail)
            .Select(user => new UserResource(user))
            .ToArray();

        return Json(users);
    }
}

