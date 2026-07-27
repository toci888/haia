using Toci.Haia.Api.Features.Registration.Email;

namespace Toci.Haia.Api.Tests.Unit;

public class EmailRegistrationNormalizationTests
{
    [Fact]
    public void NormalizeEmail_ShouldLowercaseAndTrim()
    {
        var value = EmailRegistrationNormalization.NormalizeEmail("  User@Example.COM  ");
        Assert.Equal("user@example.com", value);
    }

    [Fact]
    public void NormalizeNickname_ShouldLowercaseAndTrim()
    {
        var value = EmailRegistrationNormalization.NormalizeNickname("  NiCk_Name  ");
        Assert.Equal("nick_name", value);
    }

    [Fact]
    public void IsValidEmail_ShouldRejectInvalid()
    {
        Assert.False(EmailRegistrationNormalization.IsValidEmail("not-an-email"));
    }
}
