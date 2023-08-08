using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Services;
using Unite.Identity.Web.Resources;

namespace Unite.Identity.Web.Controllers;

[Route("api/workers")]
[Authorize(Roles = "Root")]
public class WorkersController : Controller
{
    private readonly WorkerService _workerService;

    public WorkersController(WorkerService workerService)
    {
        _workerService = workerService;
    }

    [HttpGet("")]
    public IActionResult Get()
    {
        var worker = _workerService
            .GetAll()
            .OrderBy(worker => worker.Name)
            .Select(worker => new WorkerResource(worker))
            .ToArray();

        return Json(worker);
    }
}
