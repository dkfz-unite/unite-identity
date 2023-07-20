namespace Unite.Identity.Services.Ldap.Configuration.Options;

public interface ILdapOptions
{
    string Host { get; }
    int? Port { get; }
    string UserTargetOU { get; }
    string ServiceUserLogin { get; }
    string ServiceUserPassword { get; }
}
