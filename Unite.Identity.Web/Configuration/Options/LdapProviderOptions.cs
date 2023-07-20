using Unite.Identity.Services.Ldap.Configuration.Options;

namespace Unite.Identity.Web.Configuration.Options;

public class LdapProviderOptions : ILdapOptions
{
    public bool Active
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_LDAP_ACTIVE");

            if (string.IsNullOrWhiteSpace(option))
                return false;

            if (!bool.TryParse(option, out var value))
                throw new ArgumentException("'UNITE_LDAP_ACTIVE' environment variable has to be set to 'true' or 'false'");

            return value;
        }
    }

    public string Label
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_LDAP_LABEL");

            if (Active && string.IsNullOrWhiteSpace(option))
                throw new ArgumentException("'UNITE_LDAP_LABEL' environment variable has to be set");

            return option?.Trim();
        }
    }

    public int? Priority
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_LDAP_PRIORITY");

            if (string.IsNullOrWhiteSpace(option))
                return null;

            if (!int.TryParse(option, out var value))
                throw new ArgumentException("'UNITE_LDAP_PRIORITY' environment variable has to be set to a positive integer number");

            return value;
        }
    }

    public string Host
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_LDAP_HOST");

            if (Active && string.IsNullOrWhiteSpace(option))
                throw new ArgumentException("'UNITE_LDAP_HOST' environment variable has to be set");

            return option?.Trim();
        }
    }

    public int? Port
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_LDAP_PORT");

            if (string.IsNullOrWhiteSpace(option))
                return null;

            if (!int.TryParse(option, out var value))
                throw new ArgumentException("'UNITE_LDAP_PORT' environment variable has to be set to a positive integer number");

            return value;
        }
    }

    public string UserTargetOU
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_LDAP_USER_TARGET_OU");

            if (Active && string.IsNullOrWhiteSpace(option))
                throw new ArgumentException("'UNITE_LDAP_USER_TARGET_OU' environment variable has to be set");

            return option?.Trim();
        }
    }

    public string ServiceUserLogin
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_LDAP_SERVICE_USER_LOGIN");

            if (Active && string.IsNullOrWhiteSpace(option))
                throw new ArgumentException("'UNITE_LDAP_SERVICE_USER_LOGIN' environment variable has to be set");

            return option?.Trim();
        }
    }

    public string ServiceUserPassword
    {
        get
        {
            var option = Environment.GetEnvironmentVariable("UNITE_LDAP_SERVICE_USER_PASSWORD");

            if (Active && string.IsNullOrWhiteSpace(option))
                throw new ArgumentException("'UNITE_LDAP_SERVICE_USER_PASSWORD' environment variable has to be set");

            return option;
        }
    }
}
