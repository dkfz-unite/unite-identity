using System.Security.Cryptography;
using System.Text;

namespace Unite.Identity.Helpers;

public static class PasswordHelpers
{
    public static string GetPasswordHash(string value)
    {
        var md5 = MD5.Create();

        var bytes = Encoding.ASCII.GetBytes(value);
        var hash = md5.ComputeHash(bytes);
        var hashString = Encoding.ASCII.GetString(hash);

        return hashString;
    }
}
