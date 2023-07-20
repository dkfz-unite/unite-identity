using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Services;
using Unite.Identity.Web.Resources;

namespace Unite.Identity.Web.Controllers;

[Route("api/providers")]
public class ProvidersController : Controller
{
    private readonly ProviderService _providerService;
    private readonly ILogger _logger;


    public ProvidersController(ProviderService providerService, ILogger<ProvidersController> logger)
    {
        _providerService = providerService;
        _logger = logger;
    }


    [HttpGet("")]
    public IActionResult Get()
    {
        var providers = _providerService
            .GetProviders(provider => provider.IsActive == true)
            .OrderBy(provider => provider.Priority)
            .Select(provider => new ProviderResource(provider))
            .ToArray();

        return Json(providers);
    }
}
