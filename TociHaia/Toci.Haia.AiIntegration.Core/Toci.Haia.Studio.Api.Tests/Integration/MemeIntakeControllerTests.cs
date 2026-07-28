using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Toci.Haia.Studio.Api.Contracts;
using Toci.Haia.Studio.Api.Features.Authentication;
using Toci.Haia.Studio.Api.Features.MemeIntakes;

namespace Toci.Haia.Studio.Api.Tests.Integration;

public sealed class MemeIntakeControllerTests : IClassFixture<StudioApiWebFactory>
{
    private readonly StudioApiWebFactory _factory;

    public MemeIntakeControllerTests(StudioApiWebFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateIntake_ShouldRequireAuth()
    {
        using var appFactory = CreateFactoryWithFakeService();
        var client = appFactory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/v1/studio/meme-intakes", new CreateMemeIntakeRequest("meme.png", "image/png", 100, null));

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task CreateIntake_WithoutCsrf_ShouldReturn400()
    {
        using var appFactory = CreateFactoryWithFakeService();
        var client = appFactory.CreateClient();

        await LoginAsync(client);

        var response = await client.PostAsJsonAsync("/api/v1/studio/meme-intakes", new CreateMemeIntakeRequest("meme.png", "image/png", 100, null));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("csrf_validation_failed", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CreateIntake_ShouldReturnIntentWithoutSecrets()
    {
        using var appFactory = CreateFactoryWithFakeService();
        var client = appFactory.CreateClient();

        await LoginAsync(client);
        var csrf = await GetCsrfTokenAsync(client);

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/studio/meme-intakes")
        {
            Content = JsonContent.Create(new CreateMemeIntakeRequest("meme.png", "image/png", 100, "Draft")),
        };
        request.Headers.Add("X-CSRF-TOKEN", csrf);

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<CreateMemeIntakeResponse>();
        Assert.NotNull(payload);
        Assert.False(string.IsNullOrWhiteSpace(payload.UploadUrl));
        Assert.True(payload.UploadHeaders.ContainsKey("Content-Type"));

        var body = await response.Content.ReadAsStringAsync();
        Assert.DoesNotContain("AccessKey", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Secret", body, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ApiKey", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task StartEditorialReview_WithoutCsrf_ShouldReturn400()
    {
        using var appFactory = CreateFactoryWithFakeService();
        var client = appFactory.CreateClient();

        await LoginAsync(client);

        var response = await client.PostAsync("/api/v1/studio/meme-intakes/11111111-1111-1111-1111-111111111111/editorial-review/start", null);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("csrf_validation_failed", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task ApproveEditorialReview_ShouldReturn200()
    {
        using var appFactory = CreateFactoryWithFakeService();
        var client = appFactory.CreateClient();

        await LoginAsync(client);
        var csrf = await GetCsrfTokenAsync(client);

        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/studio/meme-intakes/11111111-1111-1111-1111-111111111111/editorial-review/approve")
        {
            Content = JsonContent.Create(new MemeEditorialDecisionRequest("ok", "note")),
        };
        request.Headers.Add("X-CSRF-TOKEN", csrf);

        var response = await client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var payload = await response.Content.ReadFromJsonAsync<MemeEditorialDecisionResponse>();
        Assert.NotNull(payload);
        Assert.Equal("approved", payload.ReviewStatus);
    }

    private WebApplicationFactory<Program> CreateFactoryWithFakeService()
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

        return _factory.WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Development");
            builder.ConfigureServices(services =>
            {
                services.AddScoped<IMemeIntakeService, FakeMemeIntakeService>();
            });
        });
    }

    private static async Task LoginAsync(HttpClient client)
    {
        var csrf = await GetCsrfTokenAsync(client);
        using var request = new HttpRequestMessage(HttpMethod.Post, "/api/v1/studio/auth/login")
        {
            Content = JsonContent.Create(new StudioLoginRequest("admin@haia.local", "Password123!")),
        };

        request.Headers.Add("X-CSRF-TOKEN", csrf);
        var response = await client.SendAsync(request);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    private static async Task<string> GetCsrfTokenAsync(HttpClient client)
    {
        var response = await client.GetFromJsonAsync<StudioCsrfTokenResponse>("/api/v1/studio/auth/csrf");
        Assert.NotNull(response);
        Assert.False(string.IsNullOrWhiteSpace(response.RequestToken));
        return response.RequestToken;
    }

    private sealed class FakeMemeIntakeService : IMemeIntakeService
    {
        public Task<CreateMemeIntakeResponse> CreateAsync(Guid accountId, CreateMemeIntakeRequest request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new CreateMemeIntakeResponse(
                IntakeId: Guid.Parse("11111111-1111-1111-1111-111111111111"),
                CandidateId: Guid.Parse("22222222-2222-2222-2222-222222222222"),
                CandidateVersionId: Guid.Parse("33333333-3333-3333-3333-333333333333"),
                AssetId: Guid.Parse("44444444-4444-4444-4444-444444444444"),
                ObjectKey: "studio/onboarding-candidates/222/original",
                UploadUrl: "https://private-r2.local/upload",
                UploadHeaders: new Dictionary<string, string> { ["Content-Type"] = "image/png" },
                UploadUrlExpiresAtUtc: DateTimeOffset.UtcNow.AddMinutes(10),
                Status: "creating_draft"));
        }

        public Task<FinalizeMemeIntakeResponse> FinalizeAsync(Guid intakeId, CancellationToken cancellationToken)
            => Task.FromResult(new FinalizeMemeIntakeResponse(intakeId, "uploaded", "image/png", 100, "hash"));

        public Task<EvaluateMemeIntakeResponse> EvaluateAsync(Guid intakeId, string correlationId, CancellationToken cancellationToken)
            => throw new NotImplementedException();

        public Task<StartEditorialReviewResponse> StartEditorialReviewAsync(Guid intakeId, Guid reviewerAccountId, CancellationToken cancellationToken)
            => Task.FromResult(new StartEditorialReviewResponse(intakeId, "ready_for_review", "editorial_review", "awaiting_review", Guid.Parse("55555555-5555-5555-5555-555555555555")));

        public Task<MemeEditorialDecisionResponse> ApproveAsync(Guid intakeId, Guid reviewerAccountId, MemeEditorialDecisionRequest request, CancellationToken cancellationToken)
            => Task.FromResult(new MemeEditorialDecisionResponse(
                IntakeId: intakeId,
                Status: "approved",
                EditorialStatus: "approved",
                ClassificationStatus: "approved",
                ReviewStatus: "approved",
                CandidateClassificationId: Guid.Parse("55555555-5555-5555-5555-555555555555"),
                ReviewedAtUtc: DateTimeOffset.UtcNow,
                ReviewedByAccountId: reviewerAccountId));

        public Task<MemeEditorialDecisionResponse> RejectAsync(Guid intakeId, Guid reviewerAccountId, MemeEditorialDecisionRequest request, CancellationToken cancellationToken)
            => Task.FromResult(new MemeEditorialDecisionResponse(
                IntakeId: intakeId,
                Status: "rejected",
                EditorialStatus: "rejected",
                ClassificationStatus: "rejected",
                ReviewStatus: "rejected",
                CandidateClassificationId: Guid.Parse("55555555-5555-5555-5555-555555555555"),
                ReviewedAtUtc: DateTimeOffset.UtcNow,
                ReviewedByAccountId: reviewerAccountId));

        public Task<GetMemeIntakeResponse> GetAsync(Guid intakeId, CancellationToken cancellationToken)
            => throw new NotImplementedException();
    }
}
