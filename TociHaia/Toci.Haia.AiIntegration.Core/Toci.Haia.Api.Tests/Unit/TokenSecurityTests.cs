using Toci.Haia.Api.Features.Registration.Email;

namespace Toci.Haia.Api.Tests.Unit;

public class TokenSecurityTests
{
    [Fact]
    public void GenerateToken_ShouldProduceBase64UrlValue()
    {
        var token = VerificationTokenGenerator.Generate();
        Assert.False(string.IsNullOrWhiteSpace(token));
        Assert.DoesNotContain("+", token);
        Assert.DoesNotContain("/", token);
        Assert.DoesNotContain("=", token);
    }

    [Fact]
    public void ComputeSha256_ShouldBeDeterministic()
    {
        var a = TokenHasher.ComputeSha256("abc");
        var b = TokenHasher.ComputeSha256("abc");
        Assert.Equal(a, b);
        Assert.True(a.Length > 10);
    }
}
