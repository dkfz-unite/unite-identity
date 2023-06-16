namespace Unite.Identity.Web.Configuration.Options;

public class RootOptions
{
    public string ProviderName
    {
        get
        {
            var name = Environment.GetEnvironmentVariable("UNITE_DEFAULT_NAME");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("'UNITE_DEFAULT_NAME' environment variable has to be set");

            return name;
        }
    }

    public string ProviderPriority
    {
        get
        {
            var priority = Environment.GetEnvironmentVariable("UNITE_DEFAULT_PRIORITY");

            if (string.IsNullOrWhiteSpace(priority))
                throw new ArgumentNullException("'UNITE_DEFAULT_PRIORITY' environment variable has to be set");

            return priority;
        }
    }

    public string UserLogin
    {
        get
        {
            var email = Environment.GetEnvironmentVariable("UNITE_ROOT_USER");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException("'UNITE_ROOT_USER' environment variable has to be set");

            return email.Trim().ToLower();
        }
    }

    public string UserPassword
    {
        get
        {
            var password = Environment.GetEnvironmentVariable("UNITE_ROOT_PASSWORD");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("'UNITE_ROOT_PASSWORD' environment variable has to be set");

            return password;
        }
    }

    public string UniteLdapActive
    {
        get
        {
            var password = Environment.GetEnvironmentVariable("UNITE_LDAP_ACTIVE");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("'UNITE_LDAP_ACTIVE' environment variable has to be set");

            return password;
        }
    }
}
