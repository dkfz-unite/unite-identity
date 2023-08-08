using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Constants;
using Unite.Identity.Web.Resources;
using Unite.Identity.Services;
using Unite.Identity.Web.Models;

namespace Unite.Identity.Web.Controllers;

[Route("api/user")]
[Authorize(Roles = "Root")]
public class UserController : Controller
{
    private readonly ProviderService _providerService;
    private readonly UserService _userService;


    public UserController(ProviderService providerService, UserService userService)
    {
        _providerService = providerService;
        _userService = userService;
    }
    

    [HttpGet("")]
    public IActionResult Check([FromQuery]string provider, [FromQuery]string email)
    {
        if (string.IsNullOrWhiteSpace(provider))
            return BadRequest("Provider is required");

        if (string.IsNullOrWhiteSpace(email))
            return BadRequest("Email is required");

        var providerNormalized = provider.Trim().ToLower();
        
        var emailNormalized = email.Trim().ToLower();

        var user = _userService.GetUser(user =>
            user.Provider.Name == providerNormalized &&
            user.Email == emailNormalized
        );

        return Json(user == null);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var user = _userService.GetUser(id);

        if (user == null)
            return NotFound($"User with id '{id}' was not found");

        return Json(new UserResource(user));
    }

    [HttpPost("")]
    public IActionResult Post([FromBody]AddUserModel model)
    {
        var provider = _providerService.GetProvider(model.ProviderId.Value);

        if (provider == null)
            return NotFound($"Provider with id '{model.ProviderId}' was not found");

        var requiresActivation = provider.Name == Providers.Default;

        var user = _userService.Add(model.Email, model.ProviderId.Value, !requiresActivation, false, model.Permissions);

        if (user == null)
            return BadRequest($"User with email '{model.Email}' already exists");

        return Json(new UserResource(user));
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, [FromBody]EditUserModel model)
    {
        var provider = _providerService.GetProvider(model.ProviderId.Value);

        if (provider == null)
            return NotFound($"Provider with id '{model.ProviderId}' was not found");

        var user = _userService.Update(id, model.ProviderId.Value, model.Permissions);

        if (user == null)
            return NotFound($"User with Id `{id}`was not found");

        return Json(new UserResource(user));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _userService.Delete(id);

        if (deleted == false)
            return NotFound($"User with id `{id}` was not found");

        return Ok();
    }
}
