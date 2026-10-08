using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Unite.Identity.Helpers;

public static class PasswordHelper
{
    public static string GetPasswordHash(string value)
    {
        var hasher = new PasswordHasher<string>();

        return hasher.HashPassword(null, value);
    }

    public static bool VerifyPasswordHash(string hash, string value)
    {
        if (string.IsNullOrEmpty(hash))
            return false;

        var hasher = new PasswordHasher<string>();

        try
        {
            return hasher.VerifyHashedPassword(null, hash, value) != PasswordVerificationResult.Failed;
        }
        catch (FormatException)
        {
            // Hash is not in the expected format (e.g. legacy MD5 hash).
            return false;
        }
    }

    // Deterministic hash for random tokens, which are looked up by their hash.
    public static string GetTokenHash(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        var hash = SHA256.HashData(bytes);

        return Convert.ToHexString(hash);
    }

    // Legacy MD5 hash method, not used anymore but kept for reference.
    public static string GetPasswordHasOld(string value)
    {
        var md5 = MD5.Create();

        var bytes = Encoding.ASCII.GetBytes(value);
        var hash = md5.ComputeHash(bytes);
        var hashString = Encoding.ASCII.GetString(hash);

        return hashString;
    }
}
