using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Toci.Haia.Studio.Api.Tests.Integration;

public static class JwtTokenFactory
{
    public static string Create(
        string subject,
        string? role = null,
        string? scope = null,
        DateTime? expiresAtUtc = null)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TestEnvironmentSetup.TestSigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, subject),
            new(JwtRegisteredClaimNames.UniqueName, subject),
        };

        if (!string.IsNullOrWhiteSpace(role))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        if (!string.IsNullOrWhiteSpace(scope))
        {
            claims.Add(new Claim("scope", scope));
        }

        var token = new JwtSecurityToken(
            issuer: "haia-studio",
            audience: "haia-studio-api",
            claims: claims,
            notBefore: DateTime.UtcNow.AddMinutes(-1),
            expires: expiresAtUtc ?? DateTime.UtcNow.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
