﻿using System;
using Microsoft.AspNetCore.Mvc;

namespace Unite.Identity.Web.Controllers;

[Route("api/")]
public class DefaultController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        var date = DateTime.UtcNow;

        return Json(date);
    }
}