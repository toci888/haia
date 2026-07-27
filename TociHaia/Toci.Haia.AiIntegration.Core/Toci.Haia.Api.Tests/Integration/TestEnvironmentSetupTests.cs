namespace Toci.Haia.Api.Tests.Integration;

public class TestEnvironmentSetupTests
{
    [Fact]
    public void ModuleInitializer_ShouldSetDedicatedTestDatabaseConnectionString()
    {
        var value = Environment.GetEnvironmentVariable("HAIA_DB_CONNECTION_STRING");
        Assert.False(string.IsNullOrWhiteSpace(value));
        Assert.Contains("Database=haia_test", value, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Database=Toci.Haia", value, StringComparison.OrdinalIgnoreCase);
    }
}
