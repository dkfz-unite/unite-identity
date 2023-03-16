using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Services;
using Unite.Identity.Models;

namespace Unite.Identity.Controllers;

[Route("api/identity/[controller]")]
public class SignUpController : Controller
{
    private readonly IdentityService _identityService;

    public SignUpController(IdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost]
    public IActionResult Post([FromBody] SignUpModel signUpModel)
    {
        var user = _identityService.SignUpUser(signUpModel.Email, signUpModel.Password);

        return user != null ? Ok() : BadRequest($"Email address '{signUpModel.Email}' is not in access list or already registered");
    }
}
