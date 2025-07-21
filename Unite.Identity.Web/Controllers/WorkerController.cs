using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Services;
using Unite.Identity.Web.Configuration.Options;
using Unite.Identity.Web.Helpers;
using Unite.Identity.Web.Models;
using Unite.Identity.Web.Resources;

namespace Unite.Identity.Web.Controllers;

[Route("api/worker")]
[Authorize(Roles = "Root")]
public class WorkerController : Controller
{
    protected readonly ApiOptions _apiOptions;
    private readonly WorkerService _workerService;


    public WorkerController(ApiOptions apiOptions, WorkerService workerService)
    {
        _apiOptions = apiOptions;
        _workerService = workerService;
    }


    [HttpGet("")]
    public IActionResult Check([FromQuery]string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return BadRequest("Worker name is required");

        var nameNormalized = name.Trim().ToLower();

        var worker = _workerService.Get(worker => 
            worker.Name == nameNormalized
        );

        return Json(worker == null);
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        var worker = _workerService.Get(id);

        if (worker == null)
            return NotFound($"Worker with id '{id}' was not found");

        return Json(new WorkerResource(worker));
    }

    [HttpPost("")]
    public IActionResult Add([FromBody]AddWorkerModel model)
    {
        var worker = _workerService.Add(model.Name, model.Description, permissions: model.Permissions);

        if (worker == null)
            return BadRequest("Worker with the same name already exists");

        return Json(new WorkerResource(worker));
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody]AddWorkerModel model)
    {
        var worker = _workerService.Get(id);

        if (worker == null)
            return NotFound($"Worker with id '{id}' was not found");

        worker = _workerService.Update(id, worker with { Name = model.Name, Description = model.Description }, model.Permissions);

        if (worker == null)
            return BadRequest("Worker with the same name already exists");

        return Json(new WorkerResource(worker));
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var deleted = _workerService.Delete(id);

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

        /// var workerToken = _workerTokenService.Get(id);
        /// 
        /// if (workerToken == null)
        ///    return NotFound($"Worker token with id '{id}' was not found");
        /// else
        ///     return Ok();

        return Ok();
    }

    [HttpGet("{id}/token")]
    public IActionResult GetToken(int id)
    {
        var worker = _workerService.Get(id);

        if (worker == null)
            return NotFound($"Worker with id '{id}' was not found");

        return Json(worker.Token);
    }

    [HttpPost("{id}/token")]
    public IActionResult AddToken(int id, [FromBody]AddWorkerTokenModel model)
    {
        var worker = _workerService.Get(id);

        if (worker == null)
            return NotFound($"Worker with id '{id}' was not found");

        var identity = ClaimsHelper.GetIdentity(worker);

        var tokenExpiryDate = GetTokenExpiryDate(model);

        var token = TokenHelper.GenerateAuthorizationToken(identity, _apiOptions.Key, tokenExpiryDate);

        _workerService.Update(id, worker with { Token = token, TokenExpiryDate = tokenExpiryDate });
        
        return Ok(token);
    }

    [HttpPut("{id}/token")]
    public IActionResult UpdateToken(int id, [FromBody]AddWorkerTokenModel model)
    {
        var worker = _workerService.Get(id);

        if (worker == null)
            return NotFound($"Worker with id '{id}' was not found");

        if (worker.TokenExpiryDate > DateTime.UtcNow)
            return BadRequest("Worker token has not expired yet");

        var identity = ClaimsHelper.GetIdentity(worker);

        var tokenExpiryDate = GetTokenExpiryDate(model);

        var token = TokenHelper.GenerateAuthorizationToken(identity, _apiOptions.Key, tokenExpiryDate);

        _workerService.Update(id, worker with { Token = token, TokenExpiryDate = tokenExpiryDate });

        return Ok(token);
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
