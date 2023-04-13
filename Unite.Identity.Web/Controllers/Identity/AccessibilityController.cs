using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Services;

namespace Unite.Identity.Web.Controllers.Identity;

[Route("api/access")]
public class AccessibilityController : Controller
{
    private readonly UserService _userService;

    public AccessibilityController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Get(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return BadRequest($"Request parameter {nameof(email)} is missing");
        }

        var emailNormalized = email.Trim().ToLower();

        var candidate = _userService.GetUser(user => user.Email == emailNormalized && user.IsRegistered == false);

        return candidate != null ? Ok() : NotFound($"Email '{emailNormalized}' is not in access list.");
    }
}
