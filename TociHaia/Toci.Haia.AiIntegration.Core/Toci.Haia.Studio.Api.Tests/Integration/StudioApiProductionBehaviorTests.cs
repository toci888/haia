using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Toci.Haia.Studio.Api.Tests.Integration;

public class StudioApiProductionBehaviorTests
{
    [Fact]
    public async Task SwaggerUi_ShouldNotBeAvailableInProduction()
    {
        await using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseSetting("environment", "Production");
            });

        using var client = factory.CreateClient();
        var swagger = await client.GetAsync("/swagger/index.html");
        Assert.Equal(HttpStatusCode.NotFound, swagger.StatusCode);
    }

    [Fact]
    public async Task OpenApi_ShouldNotBePublicInProduction()
    {
        await using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseSetting("environment", "Production");
            });

        using var client = factory.CreateClient();
        var openApi = await client.GetAsync("/openapi/v1.json");
        Assert.Equal(HttpStatusCode.NotFound, openApi.StatusCode);
    }
}
