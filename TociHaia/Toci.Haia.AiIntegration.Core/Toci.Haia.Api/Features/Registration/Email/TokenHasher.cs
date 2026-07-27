using System.Security.Cryptography;
using System.Text;

namespace Toci.Haia.Api.Features.Registration.Email;

public static class TokenHasher
{
    public static string ComputeSha256(string token)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(token);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }
}
