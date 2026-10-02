using System.Text.Json.Serialization;
using Unite.Essentials.Attributes;

namespace Unite.Post.Configuration.Options;

[JsonConverter(typeof(EnumAliasJsonConverter<SmtpLoginMethod>))]
public enum SmtpLoginMethod
{
    [EnumAlias("login", "Login", "LOGIN")]
    Login,
    [EnumAlias("plain", "Plain", "PLAIN")]
    Plain,
    [EnumAlias("ntlm", "Ntlm", "NTLM")]
    Ntlm
}
