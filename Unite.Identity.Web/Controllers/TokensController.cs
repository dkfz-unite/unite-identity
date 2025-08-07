using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Services;
using Unite.Identity.Web.Resources;

namespace Unite.Identity.Web.Controllers;

[Route("api/tokens")]
[Authorize(Roles = "Root")]
public class TokensController : Controller
{
    private readonly TokenService _tokenService;

    public TokensController(TokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet("")]
    public IActionResult Get()
    {
        var token = _tokenService
            .GetAll()
            .OrderBy(token => token.Name)
            .Select(token => new TokenResource(token))
            .ToArray();

        return Ok(token);
    }
}
