namespace Unite.Identity.Web.Configuration.Options;

using IIdentitySeqlOptions = Unite.Identity.Data.Services.Configuration.Options.ISqlOptions;
using IDomainSeqlOptions = Unite.Data.Context.Configuration.Options.ISqlOptions;

public class SqlOptions : IIdentitySeqlOptions, IDomainSeqlOptions
{
    public string Host => Environment.GetEnvironmentVariable("UNITE_SQL_HOST");
    public string Port => Environment.GetEnvironmentVariable("UNITE_SQL_PORT");
    public string User => Environment.GetEnvironmentVariable("UNITE_SQL_USER");
    public string Password => Environment.GetEnvironmentVariable("UNITE_SQL_PASSWORD");

    public bool IncludeErrorDetail => false;
}
