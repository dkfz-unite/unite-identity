using Unite.Identity.Web.Configuration.Extensions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Logging.AddConsole();

builder.Services.AddControllers();

builder.Services.AddServices();

builder.Services.AddCors();

builder.Services.AddAuthentication(options => options.AddJwtAuthenticationOptions())
                .AddJwtBearer(options => options.AddJwtBearerOptions());

builder.Services.AddControllers(options => options.AddMvcOptions())
                .AddJsonOptions(options => options.AddJsonOptions());
//.AddFluentValidation(); // Deprecated so obsolete

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

app.UseHsts();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(builder => builder
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials()
);

app.MapControllers();

app.Run();