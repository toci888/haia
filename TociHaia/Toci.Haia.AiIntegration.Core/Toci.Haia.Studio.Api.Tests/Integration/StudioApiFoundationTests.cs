using System.Net;
using System.Text.Json;
using System.Net.Http.Json;
using Toci.Haia.Studio.Api.Features.Authentication;

namespace Toci.Haia.Studio.Api.Tests.Integration;

public class StudioApiFoundationTests : IClassFixture<StudioApiWebFactory>
{
    private readonly HttpClient _client;
    private readonly StudioApiWebFactory _factory;

    public StudioApiFoundationTests(StudioApiWebFactory factory)
    {
        _factory = factory;
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
        _factory.StubAuthenticationService.SessionIsValid = false;
        var response = await _client.GetAsync("/api/v1/studio/auth/status");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithoutCsrf_ShouldReturnSafeError()
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/studio/auth/login")
        {
            Content = JsonContent.Create(new { email = "admin@haia.local", password = "Password123!" })
        };
        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("csrf_validation_failed", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ShouldReturnGeneric401()
    {
        _factory.StubAuthenticationService.LoginResult = StudioLoginResult.Failed(StudioLoginFailureReason.InvalidPassword);
        var csrf = await GetCsrfTokenAsync();

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/studio/auth/login")
        {
            Content = JsonContent.Create(new { email = "admin@haia.local", password = "bad-pass" })
        };
        request.Headers.Add("X-CSRF-TOKEN", csrf);

        var response = await _client.SendAsync(request);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("invalid_credentials", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("invalid password", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldSetCookie_And_StatusShouldReturn200()
    {
        _factory.StubAuthenticationService.LoginResult = StudioLoginResult.Success(
            new StudioAuthenticatedSession(
                AccountId: Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                DisplayName: "Studio Admin",
                Email: "admin@haia.local",
                Roles: ["StudioAdmin"],
                Scopes: ["studio.api"],
                AccountStatus: "active"));
        _factory.StubAuthenticationService.SessionIsValid = true;

        var csrf = await GetCsrfTokenAsync();
        using var loginRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/studio/auth/login")
        {
            Content = JsonContent.Create(new { email = "admin@haia.local", password = "Password123!" })
        };
        loginRequest.Headers.Add("X-CSRF-TOKEN", csrf);

        var loginResponse = await _client.SendAsync(loginRequest);
        Assert.Equal(HttpStatusCode.NoContent, loginResponse.StatusCode);
        Assert.True(loginResponse.Headers.TryGetValues("Set-Cookie", out var setCookieValues));
        var sessionCookie = setCookieValues.Single(value => value.Contains("haia.studio.session", StringComparison.OrdinalIgnoreCase));
        Assert.Contains("httponly", sessionCookie, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("samesite=lax", sessionCookie, StringComparison.OrdinalIgnoreCase);

        var statusResponse = await _client.GetAsync("/api/v1/studio/auth/status");
        Assert.Equal(HttpStatusCode.OK, statusResponse.StatusCode);

        var json = await statusResponse.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        Assert.Equal("authorized", root.GetProperty("status").GetString());
        Assert.Equal("admin@haia.local", root.GetProperty("email").GetString());
        Assert.Equal("Studio Admin", root.GetProperty("user").GetString());
        Assert.Contains("StudioAdmin", root.GetProperty("roles").EnumerateArray().Select(x => x.GetString()));
        Assert.Contains("studio.api", root.GetProperty("scopes").EnumerateArray().Select(x => x.GetString()));
    }

    [Fact]
    public async Task Logout_WithoutCsrf_ShouldBeRejected()
    {
        _factory.StubAuthenticationService.LoginResult = StudioLoginResult.Success(
            new StudioAuthenticatedSession(
                AccountId: Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                DisplayName: "Studio Admin",
                Email: "admin@haia.local",
                Roles: ["StudioAdmin"],
                Scopes: ["studio.api"],
                AccountStatus: "active"));
        _factory.StubAuthenticationService.SessionIsValid = true;

        var csrf = await GetCsrfTokenAsync();
        using var loginRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/studio/auth/login")
        {
            Content = JsonContent.Create(new { email = "admin@haia.local", password = "Password123!" })
        };
        loginRequest.Headers.Add("X-CSRF-TOKEN", csrf);
        var loginResponse = await _client.SendAsync(loginRequest);
        Assert.Equal(HttpStatusCode.NoContent, loginResponse.StatusCode);

        using var logoutRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/studio/auth/logout");
        var logoutResponse = await _client.SendAsync(logoutRequest);
        Assert.Equal(HttpStatusCode.BadRequest, logoutResponse.StatusCode);
    }

    [Fact]
    public async Task Logout_WithCsrf_ShouldClearSession_And_StatusShouldReturn401()
    {
        _factory.StubAuthenticationService.LoginResult = StudioLoginResult.Success(
            new StudioAuthenticatedSession(
                AccountId: Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                DisplayName: "Studio Admin",
                Email: "admin@haia.local",
                Roles: ["StudioAdmin"],
                Scopes: ["studio.api"],
                AccountStatus: "active"));
        _factory.StubAuthenticationService.SessionIsValid = true;

        var csrf = await GetCsrfTokenAsync();
        using var loginRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/studio/auth/login")
        {
            Content = JsonContent.Create(new { email = "admin@haia.local", password = "Password123!" })
        };
        loginRequest.Headers.Add("X-CSRF-TOKEN", csrf);
        var loginResponse = await _client.SendAsync(loginRequest);
        Assert.Equal(HttpStatusCode.NoContent, loginResponse.StatusCode);

        var logoutCsrf = await GetCsrfTokenAsync();
        using var logoutRequest = new HttpRequestMessage(HttpMethod.Post, "/api/v1/studio/auth/logout");
        logoutRequest.Headers.Add("X-CSRF-TOKEN", logoutCsrf);

        var logoutResponse = await _client.SendAsync(logoutRequest);
        Assert.Equal(HttpStatusCode.NoContent, logoutResponse.StatusCode);

        _factory.StubAuthenticationService.SessionIsValid = false;
        var statusResponse = await _client.GetAsync("/api/v1/studio/auth/status");
        Assert.Equal(HttpStatusCode.Unauthorized, statusResponse.StatusCode);
    }

    private async Task<string> GetCsrfTokenAsync()
    {
        var csrfResponse = await _client.GetAsync("/api/v1/studio/auth/csrf");
        Assert.Equal(HttpStatusCode.OK, csrfResponse.StatusCode);

        var csrfBody = await csrfResponse.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(csrfBody);
        return document.RootElement.GetProperty("requestToken").GetString()
            ?? throw new InvalidOperationException("Missing csrf request token");
    }
}
