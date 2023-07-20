using System.DirectoryServices.Protocols;
using System.Net;
using Unite.Identity.Services.Ldap.Configuration.Options;
using Unite.Identity.Services.Ldap.Models;

namespace Unite.Identity.Services.Ldap;

public class LdapService
{
    private readonly ILdapOptions _options;


    public LdapService(ILdapOptions options)
    {
        _options = options;
    }


    public LdapUser FindUser(string login)
    {
        using var connection = CreateConnection(_options, _options.ServiceUserLogin, _options.ServiceUserPassword);

        try
        {
            connection.Bind();

            var isEmail = login.Contains("@");
            var property = isEmail ? "mail" : "cn";
            var filter = $"({property}={login})";
            var attributes = new string[] { "DistinguishedName", "Mail" };
            
            var request = new SearchRequest(_options.UserTargetOU, filter, SearchScope.Subtree, attributes);
            var response = (SearchResponse)connection.SendRequest(request);

            return response.Entries.Count > 0 ? new LdapUser(response.Entries[0]) : null;
        }
        catch
        {
            throw;
        }
    }

    public bool AuthenticateUser(string login, string password)
    {
        using var connection = CreateConnection(_options, login, password);

        try
        {
            connection.Bind();

            return true;
        }
        catch
        {
            return false;
        }
    }


    private LdapConnection CreateConnection(ILdapOptions options, string login, string password)
    {
        var diractory = options.Port.HasValue 
            ? new LdapDirectoryIdentifier(options.Host, options.Port.Value) 
            : new LdapDirectoryIdentifier(options.Host);

        var credentials = new NetworkCredential(login, password);
        
        return new LdapConnection(diractory, credentials, AuthType.Basic);
    }
}
