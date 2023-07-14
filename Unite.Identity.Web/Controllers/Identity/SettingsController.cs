using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Services;
using System.Data;
using Unite.Identity.Resources;

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
            .OrderBy(provider => provider.Priority)
            .Select(provider => new ProviderResource(provider))
            .ToArray();

        return Json(providers);
    }
}

