using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Unite.Identity.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Logging.AddConsole();


//builder.Services.AddSpaStaticFiles(configuration =>
//{
//    configuration.RootPath = "Client/dist";
//});

builder.Services.AddControllers();

builder.Services.AddTransient<LDAPAuthenticationService>();
builder.Services.AddTransient<UniteAuthenticationService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<IdentityService>();
builder.Services.AddTransient<SessionOptions>();

builder.Services.AddCors();

byte[] key = System.Text.Encoding.UTF8.GetBytes("ToDo"/*Environment.GetEnvironmentVariable("API_KEY")*/);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Data.Manager", policy => policy
            .RequireClaim("permission", "Data.Write")
            .RequireClaim("permission", "Data.Edit")
            .RequireClaim("permission", "Data.Delete")
        );
});

var app = builder.Build();

app.UseHsts();

if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Release")
{
    app.UseHttpsRedirection();
}

//app.UseSpaStaticFiles();

//app.UseSpa(spa =>
//{
//    spa.Options.SourcePath = "Client";
//});

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