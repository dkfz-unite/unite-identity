using System;
using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Claims;
using Unite.Identity.Web.Helpers;
using Unite.Identity.Models;
using Unite.Identity.Web.Configuration.Options;

namespace Unite.Identity.Web.Controllers.Identity;

[Route("api/settings")]
public class SettingsController : Controller
{
    private readonly ProviderService _providerService;

    public SettingsController(
        ProviderService providerService)
    {
        _providerService = providerService;
    }


    [HttpGet("providers")]
    public IActionResult ActiveProviders()
    {
        var providers = _providerService
            .GetProviders(provider => provider.IsActive == true)
            .Select(provider => new ProviderModel(provider))
            .ToArray();

        return Json(providers);
    }
}

