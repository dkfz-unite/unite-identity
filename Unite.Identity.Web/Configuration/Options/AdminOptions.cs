namespace Unite.Identity.Web.Configuration.Options;

public class AdminOptions
{
    public string Login
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_ADMIN_USER");

            if (string.IsNullOrWhiteSpace(option))
                throw new ArgumentNullException("'UNITE_ADMIN_USER' environment variable has to be set");

            return option.Trim().ToLower();
        }
    }

    public string Password
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_ADMIN_PASSWORD");

            if (string.IsNullOrWhiteSpace(option))
                throw new ArgumentNullException("'UNITE_ADMIN_PASSWORD' environment variable has to be set");

            return option;
        }
    }
}
