using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Services;
using Unite.Identity.Web.Configuration.Options;
using Unite.Identity.Web.Helpers;
using Unite.Identity.Web.Models;
using Unite.Identity.Web.Resources;

namespace Unite.Identity.Web.Controllers;

[Route("api/token")]
[Authorize(Roles = "Root")]
public class TokenController : Controller
{
    protected readonly ApiOptions _apiOptions;
    private readonly TokenService _tokenService;


    public TokenController(ApiOptions apiOptions, TokenService TokenService)
    {
        _apiOptions = apiOptions;
        _tokenService = TokenService;
    }


    [HttpGet("")]
    public IActionResult Check([FromQuery]string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Token name is required");

        var nameNormalized = name.Trim().ToLower();

        var token = _tokenService.Get(token => 
            token.Name == nameNormalized
        );

        return Ok(token == null);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var token = _tokenService.Get(id);

        if (token == null)
            return NotFound($"Token with id '{id}' was not found");

        return Ok(new TokenResource(token));
    }

    [HttpPost("")]
    public IActionResult Add([FromBody]AddTokenModel model)
    {
        var expiryDate = GetTokenExpiryDate(model.ExpiryDate);

        var token = _tokenService.Add(model.Name, expiryDate, model.Permissions, model.Description);

        if (token == null)
            return BadRequest("Token with the same name already exists");

        var identity = ClaimsHelper.GetIdentity(token);

        var value = TokenHelper.GenerateAuthorizationToken(identity, _apiOptions.Key, expiryDate);

        return Ok(value);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody]EditTokenModel model)
    {
        var token = _tokenService.Get(id);

        if (token == null)
            return NotFound($"Token with id '{id}' was not found");

        var expiryDate = GetTokenExpiryDate(model.ExpiryDate);

        token = _tokenService.Update(id, token with { ExpiryDate = expiryDate }, model.Permissions);

        var identity = ClaimsHelper.GetIdentity(token);

        var value = TokenHelper.GenerateAuthorizationToken(identity, _apiOptions.Key, expiryDate);

        return Ok(value);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _tokenService.Delete(id);

        if (!deleted)
            return NotFound($"Token with id '{id}' was not found");

        return Ok();
    }


    [AllowAnonymous]
    [HttpGet("{key}/active")]
    public IActionResult GetStatus(string key)
    {
        var active = _tokenService.IsActive(key);
        
        return active ? Ok() : NotFound($"Active token with key '{key}' was not found");
    }

    [HttpPut("{id}/revoke")]
    public IActionResult Revoke(int id)
    {
        var token = _tokenService.Get(id);

        if (token == null)
            return NotFound($"Token with id '{id}' was not found");

        if (token.ExpiryDate < DateTime.UtcNow)
            return BadRequest("Token has already expired");

        token.Revoked = true;

        _tokenService.Update(id, token);

        return Ok();
    }
    

    private static DateTime GetTokenExpiryDate(EpiryDateModel model)
    {
        var expiryDate = DateTime.UtcNow;

        if (model.ExpiryMinutes.HasValue)
            expiryDate = expiryDate.AddMinutes(model.ExpiryMinutes.Value);
        else if (model.ExpiryHours.HasValue)
            expiryDate = expiryDate.AddHours(model.ExpiryHours.Value);
        else if (model.ExpiryDays.HasValue)
            expiryDate = expiryDate.AddDays(model.ExpiryDays.Value);

        return expiryDate;
    }
}
