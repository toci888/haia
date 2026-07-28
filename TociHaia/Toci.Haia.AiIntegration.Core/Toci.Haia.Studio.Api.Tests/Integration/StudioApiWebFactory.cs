using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Toci.Haia.Studio.Api.Features.Authentication;

namespace Toci.Haia.Studio.Api.Tests.Integration;

public sealed class StudioApiWebFactory : WebApplicationFactory<Program>
{
    public StubStudioAuthenticationService StubAuthenticationService { get; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(StubAuthenticationService);
            services.AddSingleton<IStudioAuthenticationService>(sp => sp.GetRequiredService<StubStudioAuthenticationService>());
        });
    }
}
