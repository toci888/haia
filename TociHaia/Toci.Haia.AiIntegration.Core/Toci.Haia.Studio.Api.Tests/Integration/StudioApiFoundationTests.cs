using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Toci.Haia.Studio.Api.Tests.Integration;

public class StudioApiFoundationTests : IClassFixture<StudioApiWebFactory>
{
    private readonly HttpClient _client;

    public StudioApiFoundationTests(StudioApiWebFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task HealthLive_ShouldReturn200()
    {
        var response = await _client.GetAsync("/health/live");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SystemInfo_ShouldReturnExpectedDto()
    {
        var response = await _client.GetAsync("/api/v1/system/info");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        Assert.Equal("HAIA Studio API", root.GetProperty("serviceName").GetString());
        Assert.Equal("ok", root.GetProperty("status").GetString());
        Assert.True(root.TryGetProperty("apiVersion", out _));
    }

    [Fact]
    public async Task Response_ShouldContainCorrelationId()
    {
        var response = await _client.GetAsync("/api/v1/system/info");
        Assert.True(response.Headers.Contains("X-Correlation-ID"));
    }

    [Fact]
    public async Task ValidProvidedCorrelationId_ShouldBePreserved()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/system/info");
        request.Headers.Add("X-Correlation-ID", "corr-abc-123");
        var response = await _client.SendAsync(request);
        var echoed = response.Headers.GetValues("X-Correlation-ID").Single();
        Assert.Equal("corr-abc-123", echoed);
    }

    [Fact]
    public async Task OpenApi_ShouldContainStudioSystemAction()
    {
        var response = await _client.GetAsync("/openapi/v1.json");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("/api/v1/system/info", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task OpenApi_ShouldNotExposeEfTypes()
    {
        var response = await _client.GetAsync("/openapi/v1.json");
        var body = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("HaiaDbContext", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Toci.Haia.Database.Persistence.Entities", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task SwaggerUi_ShouldBeAvailableInDevelopment()
    {
        var response = await _client.GetAsync("/swagger/index.html");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task NoAdministrativeBusinessEndpoints_ShouldExistYet()
    {
        var response = await _client.GetAsync("/api/v1/candidates");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AuthStatus_WithoutToken_ShouldReturn401()
    {
        var response = await _client.GetAsync("/api/v1/studio/auth/status");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AuthStatus_WithInvalidToken_ShouldReturn401()
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/studio/auth/status");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "invalid-token");
        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AuthStatus_WithMissingRoleOrScope_ShouldReturn403()
    {
        var token = JwtTokenFactory.Create(subject: "editor@haia.local", role: "StudioEditor", scope: "studio.api");
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/studio/auth/status");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task AuthStatus_WithValidStudioAccessToken_ShouldReturn200()
    {
        var token = JwtTokenFactory.Create(subject: "admin@haia.local", role: "StudioAdmin", scope: "studio.api");
        using var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/studio/auth/status");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        Assert.Equal("authorized", root.GetProperty("status").GetString());
        Assert.Contains("StudioAdmin", root.GetProperty("roles").EnumerateArray().Select(x => x.GetString()));
    }
}
