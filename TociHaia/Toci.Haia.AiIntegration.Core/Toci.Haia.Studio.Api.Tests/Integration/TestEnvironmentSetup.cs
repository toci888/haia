using System.Runtime.CompilerServices;

namespace Toci.Haia.Studio.Api.Tests.Integration;

public static class TestEnvironmentSetup
{
    public const string TestSigningKey = "haia-studio-test-signing-key-1234567890";

    [ModuleInitializer]
    public static void Initialize()
    {
        Environment.SetEnvironmentVariable(
            "HAIA_DB_CONNECTION_STRING",
            "Host=localhost;Port=5433;Database=haia_test;Username=haia;Password=haia");

        Environment.SetEnvironmentVariable(
            "HAIA_STUDIO_AUTH_SIGNING_KEY",
            TestSigningKey);
    }
}
