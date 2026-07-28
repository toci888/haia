using System.Runtime.CompilerServices;

namespace Toci.Haia.Studio.Api.Tests.Integration;

public static class TestEnvironmentSetup
{
    [ModuleInitializer]
    public static void Initialize()
    {
        Environment.SetEnvironmentVariable(
            "HAIA_DB_CONNECTION_STRING",
            "Host=localhost;Port=5433;Database=haia_test;Username=haia;Password=haia");
    }
}
