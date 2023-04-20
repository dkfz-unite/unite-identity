using Microsoft.AspNetCore.Mvc;
using Unite.Identity.Web.Configuration.Filters;

namespace Unite.Identity.Web.Configuration.Extensions;

public static class MvcOptionsExtensions
{
    public static void AddMvcOptions(this MvcOptions options)
    {
        options.Filters.Add(typeof(DefaultActionFilter));
        options.Filters.Add(typeof(DefaultExceptionFilter));
    }
}