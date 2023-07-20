using System.DirectoryServices.Protocols;

namespace Unite.Identity.Services.Ldap.Models;

public class LdapUser
{
    private const string EmailAttribute = "mail";

    public string Login { get; set; }
    public string Email { get; set; }


    public LdapUser(SearchResultEntry entry)
    {
        Login = entry.DistinguishedName;
        Email =  GetAttributeValue(entry.Attributes, EmailAttribute);

    }


    private string GetAttributeValue(SearchResultAttributeCollection attributes, string name)
    {
        return attributes.Contains(name) ? attributes[name][0].ToString() : null;
    }
}
