using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Unite.Identity.Helpers;

public class TokenHelper
{
    private const int TOKEN_EXPIRY_MINUTES = 15;

    private static readonly JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();


    public static string GenerateAuthorizationToken(ClaimsIdentity identity, byte[] key)
    {
        var securityKey = new SymmetricSecurityKey(key);
        var securityAlgorythm = SecurityAlgorithms.HmacSha256Signature;
        var credentials = new SigningCredentials(securityKey, securityAlgorythm);

        var descriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = DateTime.UtcNow.AddMinutes(TOKEN_EXPIRY_MINUTES),
            SigningCredentials = credentials
        };

        var token = _tokenHandler.CreateToken(descriptor);
        var tokenString = _tokenHandler.WriteToken(token);

        return tokenString;
    }
}
