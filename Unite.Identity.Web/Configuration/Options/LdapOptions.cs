using Unite.Identity.Data.Services.Configuration.Options;

namespace Unite.Identity.Web.Configuration.Options;

public class LdapOptions : ILdapOptions
{
    public string Server
    {
        get
        {
            var server = Environment.GetEnvironmentVariable("UNITE_LDAP_SERVER");

            if (string.IsNullOrWhiteSpace(server))
                throw new ArgumentNullException("'UNITE_LDAP_SERVER' environment variable has to be set");

            return server;
        }
    }

    public string Port => Environment.GetEnvironmentVariable("UNITE_LDAP_PORT");

    public string UserTargetOU
    {
        get
        {
            var target = Environment.GetEnvironmentVariable("UNITE_LDAP_USER_TARGET_OU");

            if (string.IsNullOrWhiteSpace(target))
                throw new ArgumentNullException("'UNITE_LDAP_USER_TARGET_OU' environment variable has to be set");

            return target;
        }
    }

    public string ServiceUserRNA
    {
        get
        {
            var rna = Environment.GetEnvironmentVariable("UNITE_LDAP_SERVICE_USER_RNA");

            if (string.IsNullOrWhiteSpace(rna))
                throw new ArgumentNullException("'UNITE_LDAP_SERVICE_USER_RNA' environment variable has to be set");

            return rna;
        }
    }

    public string ServiceUserPassword
    {
        get
        {
            var password = Environment.GetEnvironmentVariable("UNITE_LDAP_SERVICE_USER_PASSWORD");

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentNullException("'UNITE_LDAP_SERVICE_USER_PASSWORD' environment variable has to be set");

            return password;
        }
    }
}
