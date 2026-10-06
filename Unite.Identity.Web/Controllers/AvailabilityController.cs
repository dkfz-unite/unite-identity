using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Web.Configuration.Extensions;
using Unite.Post.Configuration.Options;

namespace Unite.Identity.Web.Controllers;

[Route("api/availability")]
[AllowAnonymous]
public class AvailabilityController : Controller
{
    private readonly ISmtpOptions _smtpOptions;

    public AvailabilityController(ISmtpOptions smtpOptions)
    {
        _smtpOptions = smtpOptions;
    }

    [HttpGet("password-reset")]
    public IActionResult PasswordReset()
    {
        return Ok(_smtpOptions.IsConfigured());
    }
}
