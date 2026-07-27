using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Toci.Haia.Api.Controllers;
using Toci.Haia.Api.Features.LegalDocuments.Contracts;
using Toci.Haia.Api.Features.Registration.Email.Contracts;
using Toci.Haia.Api.Features.Registration.Nickname.Contracts;

namespace Toci.Haia.Api.Tests.Integration;

public class ApiHttpRefactorTests
{
    [Fact]
    public async Task BusinessRoutes_ShouldBeReachable_AfterControllerRefactor()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var legal = await client.GetAsync("/api/v1/legal-documents/current");
        Assert.True(legal.StatusCode is HttpStatusCode.OK or HttpStatusCode.InternalServerError or HttpStatusCode.ServiceUnavailable);

        var nickname = await client.GetAsync("/api/v1/registration/nickname-availability?nickname=test-user");
        Assert.True(nickname.StatusCode is HttpStatusCode.OK or HttpStatusCode.InternalServerError);

        var registerBody = JsonSerializer.Serialize(new RegisterEmailRequest("user@example.com", "StrongPassword123!", "tester01", "pl-PL", []));
        using var registerContent = new StringContent(registerBody, Encoding.UTF8, "application/json");
        var register = await client.PostAsync("/api/v1/registration/email", registerContent);
        Assert.True(register.StatusCode is HttpStatusCode.Accepted or HttpStatusCode.Conflict or HttpStatusCode.UnprocessableEntity or HttpStatusCode.ServiceUnavailable or HttpStatusCode.InternalServerError);

        var verifyBody = JsonSerializer.Serialize(new VerifyEmailRequest("invalid-token"));
        using var verifyContent = new StringContent(verifyBody, Encoding.UTF8, "application/json");
        var verify = await client.PostAsync("/api/v1/registration/email/verify", verifyContent);
        Assert.True(verify.StatusCode is HttpStatusCode.NoContent or HttpStatusCode.BadRequest or HttpStatusCode.InternalServerError);

        var resendBody = JsonSerializer.Serialize(new ResendEmailRequest("user@example.com", "pl-PL"));
        using var resendContent = new StringContent(resendBody, Encoding.UTF8, "application/json");
        var resend = await client.PostAsync("/api/v1/registration/email/resend", resendContent);
        Assert.True(resend.StatusCode is HttpStatusCode.Accepted or HttpStatusCode.InternalServerError);
    }

    [Fact]
    public void BusinessRoutes_ShouldNotBeDuplicated_InEndpointTable()
    {
        using var factory = new WebApplicationFactory<Program>();
        using var scope = factory.Services.CreateScope();
        var dataSources = scope.ServiceProvider.GetServices<Microsoft.AspNetCore.Routing.EndpointDataSource>();

        var businessRoutes = dataSources
            .SelectMany(ds => ds.Endpoints)
            .OfType<Microsoft.AspNetCore.Routing.RouteEndpoint>()
            .Select(ep => ep.RoutePattern.RawText)
            .Where(t => t is not null)
            .ToList();

        Assert.Equal(1, businessRoutes.Count(t => t == "api/v1/legal-documents/current"));
        Assert.Equal(1, businessRoutes.Count(t => t == "api/v1/registration/nickname-availability"));
        Assert.Equal(1, businessRoutes.Count(t => t == "api/v1/registration/email"));
        Assert.Equal(1, businessRoutes.Count(t => t == "api/v1/registration/email/verify"));
        Assert.Equal(1, businessRoutes.Count(t => t == "api/v1/registration/email/resend"));
    }

    [Fact]
    public async Task OpenApi_ShouldContain_ControllerRoutes()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/openapi/v1.json");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        Assert.Contains("/api/v1/legal-documents/current", json, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("/api/v1/registration/nickname-availability", json, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("/api/v1/registration/email", json, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("/api/v1/registration/email/verify", json, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("/api/v1/registration/email/resend", json, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task SwaggerUi_ShouldBeAvailable_InDevelopment()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/swagger/index.html");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SwaggerUi_ShouldNotBeEnabled_InProduction()
    {
        await using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.UseEnvironment("Production"));
        using var client = factory.CreateClient();

        var swagger = await client.GetAsync("/swagger/index.html");
        Assert.Equal(HttpStatusCode.NotFound, swagger.StatusCode);

        var openApi = await client.GetAsync("/openapi/v1.json");
        Assert.Equal(HttpStatusCode.NotFound, openApi.StatusCode);
    }

    [Fact]
    public void Controllers_ShouldUseDomainDtos_NotPersistenceEntities()
    {
        var legalMethod = typeof(LegalDocumentsController).GetMethod("GetCurrent");
        Assert.NotNull(legalMethod);
        Assert.Equal(typeof(Task<ActionResult<CurrentLegalDocumentsResponse>>), legalMethod!.ReturnType);

        var registrationType = typeof(RegistrationController);
        var registerMethod = registrationType.GetMethod("RegisterByEmail");
        Assert.NotNull(registerMethod);
        var registerParam = registerMethod!.GetParameters().First(p => p.ParameterType == typeof(RegisterEmailRequest));
        Assert.Equal(typeof(RegisterEmailRequest), registerParam.ParameterType);

        Assert.DoesNotContain(registrationType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly),
            m => m.ReturnType.Assembly.GetName().Name?.Contains("Toci.Haia.Database.Persistence", StringComparison.OrdinalIgnoreCase) == true);
    }

    [Fact]
    public async Task RegistrationEndpoints_ShouldKeepExpectedHttpStatusContracts()
    {
        await using var factory = new WebApplicationFactory<Program>();
        using var client = factory.CreateClient();

        var verifyBody = JsonSerializer.Serialize(new VerifyEmailRequest("invalid-token"));
        using var verifyContent = new StringContent(verifyBody, Encoding.UTF8, "application/json");
        var verify = await client.PostAsync("/api/v1/registration/email/verify", verifyContent);
        Assert.Contains(verify.StatusCode, new[] { HttpStatusCode.NoContent, HttpStatusCode.BadRequest, HttpStatusCode.InternalServerError });

        var resendBody = JsonSerializer.Serialize(new ResendEmailRequest("user@example.com", "pl-PL"));
        using var resendContent = new StringContent(resendBody, Encoding.UTF8, "application/json");
        var resend = await client.PostAsync("/api/v1/registration/email/resend", resendContent);
        Assert.Contains(resend.StatusCode, new[] { HttpStatusCode.Accepted, HttpStatusCode.InternalServerError });
    }
}
