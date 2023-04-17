namespace Unite.Identity.Web.Configuration.Options;

public class RootOptions
{
    public string User
    {
        get
        {
            var email = Environment.GetEnvironmentVariable("UNITE_ROOT_USER");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("'UNITE_ROOT_USER' environment variable has to be set");

            return email.Trim().ToLower();
        }
    }

    public string Password
    {
        get
        {
            var password = Environment.GetEnvironmentVariable("UNITE_ROOT_PASSWORD");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("'UNITE_ROOT_PASSWORD' environment variable has to be set");

            return password;
        }
    }
}
