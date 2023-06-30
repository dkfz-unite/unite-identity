namespace Unite.Identity.Web.Configuration.Options;

public class RootOptions
{
    public string DefaultProviderLabel
    {
        get
        {
            var label = Environment.GetEnvironmentVariable("UNITE_DEFAULT_LABEL");

            if (string.IsNullOrWhiteSpace(label))
                throw new ArgumentNullException("'UNITE_DEFAULT_LABEL' environment variable has to be set");

            return label;
        }
    }

    public string DefaultProviderPriority
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

    public string LdapProviderActive
    {
        get
        {
            var active = Environment.GetEnvironmentVariable("UNITE_LDAP_ACTIVE");

            if (string.IsNullOrWhiteSpace(active))
                throw new ArgumentNullException("'UNITE_LDAP_ACTIVE' environment variable has to be set");

            return active;
        }
    }

    public string LdapProviderLabel
    {
        get
        {
            var label = Environment.GetEnvironmentVariable("UNITE_LDAP_LABEL");

            if (string.IsNullOrWhiteSpace(label))
                throw new ArgumentNullException("'UNITE_LDAP_LABEL' environment variable has to be set");

            return label;
        }
    }

    public string LdapProviderPriority
    {
        get
        {
            var priority = Environment.GetEnvironmentVariable("UNITE_LDAP_PRIORITY");

            if (string.IsNullOrWhiteSpace(priority))
                throw new ArgumentNullException("'UNITE_LDAP_PRIORITY' environment variable has to be set");

            return priority;
        }
    }
}
