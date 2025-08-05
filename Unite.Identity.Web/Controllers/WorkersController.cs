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

    public TokensController(TokenService workerService)
    {
        _tokenService = workerService;
    }

    [HttpGet("")]
    public IActionResult Get()
    {
        var worker = _tokenService
            .GetAll()
            .OrderBy(worker => worker.Name)
            .Select(worker => new TokenResource(worker))
            .ToArray();

        return Json(worker);
    }
}
