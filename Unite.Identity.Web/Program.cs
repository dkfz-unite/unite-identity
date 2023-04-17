using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Unite.Identity.Data.Services;
using Unite.Identity.Data.Services.Configuration.Options;
using Unite.Identity.Services;
using Unite.Identity.Web.Configuration.Options;
using Unite.Identity.Web.Configuration.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Logging.AddConsole();

builder.Services.AddControllers();

builder.Services.AddServices();

builder.Services.AddCors();

builder.Services.AddAuthentication(options => options.AddJwtAuthenticationOptions())
                .AddJwtBearer(options => options.AddJwtBearerOptions());

builder.Services.AddAuthorization(options => options.AddAuthorizationOptions());

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