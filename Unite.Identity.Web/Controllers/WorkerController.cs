using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Data.Entities;
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
            return BadRequest("Worker name is required");

        var nameNormalized = name.Trim().ToLower();

        var token = _tokenService.Get(token => 
            token.Name == nameNormalized
        );

        return Json(token == null);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var token = _tokenService.Get(id);

        if (token == null)
            return NotFound($"Worker with id '{id}' was not found");

        return Json(new TokenResource(token));
    }

    [HttpPost("")]
    public IActionResult Add([FromBody]AddWorkerModel model)
    {
        var token = new Token
        {
            Name = model.Name,
            Description = model.Description,
            TokenPermissions = model.Permissions?.Select(permission => new TokenPermission
            {
                PermissionId = permission
            }).ToArray(),
        };
        var expiryDate = GetTokenExpiryDate(model.ExpiryDate);
        var identity = ClaimsHelper.GetIdentity(token);
        var value = TokenHelper.GenerateAuthorizationToken(identity, _apiOptions.Key, expiryDate);
        token = _tokenService.Add(model.Name, expiryDate, permissions: model.Permissions, model.Description);

        if (token == null)
            return BadRequest("Worker with the same name already exists");

        return Json(value);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody]AddWorkerModel model)
    {
        var token = _tokenService.Get(id);

        if (token == null)
            return NotFound($"Worker with id '{id}' was not found");

        token = _tokenService.Update(id, token with { Name = model.Name, Description = model.Description, Revoked = model.Revoked }, model.Permissions);

        if (token == null)
            return BadRequest("Worker with the same name already exists");

        return Json(new TokenResource(token));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _tokenService.Delete(id);

        if (!deleted)
            return NotFound($"Worker with id '{id}' was not found");

        return Ok();
    }


    [AllowAnonymous]
    [HttpGet("token/{id}/active")]
    public IActionResult GetTokenStatus(int id)
    {
        // TODO: Check if the worker token exists.
        // Id here is the ID of the worker token.
        // If Yes - return OK, if not (it was revoked/removed) - return NotFound.

        var tokenToken = _tokenService.GetToken(id);//key
        //status - revoked, expired.
        
        if (tokenToken == null)
            return NotFound($"Worker token with id '{id}' was not found");
        else
            return Ok();
    }

    // [HttpGet("{id}/token")]
    // public IActionResult GetToken(int id)
    // {
    //     var token = _tokenService.Get(id);

    //     if (token == null)
    //         return NotFound($"Worker with id '{id}' was not found");

    //     // return Json(token.Token);
    //     return Ok();
    // }

    // [HttpPost("{id}/token")]
    // public IActionResult AddToken(int id, [FromBody]AddWorkerTokenModel model)
    // {
    //     var token = _tokenService.Get(id);

    //     if (token == null)
    //         return NotFound($"Worker with id '{id}' was not found");

    //     var identity = ClaimsHelper.GetIdentity(token);

    //     var tokenExpiryDate = GetTokenExpiryDate(model);

    //     var value = TokenHelper.GenerateAuthorizationToken(identity, _apiOptions.Key, tokenExpiryDate);

    //     _tokenService.Update(id, token with { TokenExpiryDate = tokenExpiryDate });
        
    //     return Ok(value);
    // }

    [HttpPut("{id}/token")]
    public IActionResult UpdateToken(int id, [FromBody]EditTokenModel model)
    {
        var token = _tokenService.Get(id);

        if (token == null)
            return NotFound($"Worker with id '{id}' was not found");

        if (token.TokenExpiryDate > DateTime.UtcNow)
            return BadRequest("Worker token has not expired yet");

        var identity = ClaimsHelper.GetIdentity(token);

        var tokenExpiryDate = GetTokenExpiryDate(model.ExpiryDate);

        var value = TokenHelper.GenerateAuthorizationToken(identity, _apiOptions.Key, tokenExpiryDate);

        _tokenService.Update(id, token with { TokenExpiryDate = tokenExpiryDate }, model.Permissions);

        return Ok(value);
    }

    [HttpPut("{id}/revoke")]
    public IActionResult UpdateToken(int id)
    {
        var token = _tokenService.Get(id);

        if (token == null)
            return NotFound($"Worker with id '{id}' was not found");

        if (token.TokenExpiryDate < DateTime.UtcNow)
            return BadRequest("Token has already expired");

        token.Revoked = true;
        _tokenService.Update(id, token);

        return Ok();
    }
    

    private static DateTime GetTokenExpiryDate(AddWorkerTokenModel model)
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
