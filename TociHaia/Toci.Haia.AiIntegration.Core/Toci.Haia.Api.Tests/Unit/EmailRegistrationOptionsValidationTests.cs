using Toci.Haia.Api.Features.Registration.Email;

namespace Toci.Haia.Api.Tests.Unit;

public class EmailRegistrationOptionsValidationTests
{
    [Fact]
    public void PasswordLengthRange_ShouldAllowConfiguredDefaults()
    {
        var options = new EmailRegistrationOptions();
        Assert.True(options.MinimumPasswordLength >= 12);
        Assert.True(options.MaximumPasswordLength >= options.MinimumPasswordLength);
    }
}
