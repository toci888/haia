using System.Security.Claims;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.RateLimiting;
using Toci.Haia.Database.Persistence.Context;
using Toci.Haia.Database.Persistence.DependencyInjection;
using Toci.Haia.AiIntegration.OpenAI.DependencyInjection;
using Toci.Haia.Studio.Api.Common.Correlation;
using Toci.Haia.Studio.Api.Common.Errors;
using Toci.Haia.Studio.Api.Features.Authentication;
using Toci.Haia.Studio.Api.Features.MemeIntakes;
using Toci.Haia.Studio.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var shouldBootstrapStudioAdmin = args.Any(arg => string.Equals(arg, "--bootstrap-studio-admin", StringComparison.OrdinalIgnoreCase));

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHealthChecks();
builder.Services.AddControllers();

builder.Services
    .AddOptions<StudioRateLimitOptions>()
    .Bind(builder.Configuration.GetSection(StudioRateLimitOptions.SectionName))
    .ValidateOnStart();

builder.Services
    .AddOptions<StudioCookieAuthOptions>()
    .Bind(builder.Configuration.GetSection(StudioCookieAuthOptions.SectionName))
    .ValidateOnStart();

builder.Services
    .AddOptions<StudioPasswordPolicyOptions>()
    .Bind(builder.Configuration.GetSection(StudioPasswordPolicyOptions.SectionName))
    .ValidateOnStart();

builder.Services
    .AddOptions<MemeIntakeOptions>()
    .Bind(builder.Configuration.GetSection(MemeIntakeOptions.SectionName))
    .ValidateOnStart();

builder.Services
    .AddOptions<R2StorageOptions>()
    .Bind(builder.Configuration.GetSection(R2StorageOptions.SectionName))
    .ValidateOnStart();

var studioCookieAuth = builder.Configuration.GetSection(StudioCookieAuthOptions.SectionName).Get<StudioCookieAuthOptions>()
    ?? new StudioCookieAuthOptions();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = studioCookieAuth.CookieName;
        options.Cookie.HttpOnly = true;
        options.Cookie.Path = "/";
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
            ? CookieSecurePolicy.SameAsRequest
            : CookieSecurePolicy.Always;
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(studioCookieAuth.SessionMinutes);
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            },
            OnValidatePrincipal = context =>
            {
                var validator = context.HttpContext.RequestServices.GetRequiredService<IStudioAuthenticationService>();
                var accountStatus = context.Principal?.FindFirstValue("account_status");
                var hasRequiredScope = context.Principal?.HasClaim(c => c.Type == "scope" && c.Value == "studio.api") == true;
                var hasRequiredRole = context.Principal?.IsInRole("StudioAdmin") == true;
                var accountIdRaw = context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier);
                var hasValidAccountId = Guid.TryParse(accountIdRaw, out var accountId);

                var stillValid = hasValidAccountId
                    ? validator.ValidateSessionAsync(accountId, context.HttpContext.RequestAborted).GetAwaiter().GetResult()
                    : false;

                if (!string.Equals(accountStatus, "active", StringComparison.OrdinalIgnoreCase)
                    || !hasRequiredScope
                    || !hasRequiredRole)
                {
                    context.RejectPrincipal();
                }

                if (!stillValid)
                {
                    context.RejectPrincipal();
                }

                return Task.CompletedTask;
            },
        };
    });

builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.Name = "haia.studio.csrf";
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = builder.Environment.IsDevelopment()
        ? CookieSecurePolicy.SameAsRequest
        : CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("studio-access", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole("StudioAdmin");
        policy.RequireClaim("scope", "studio.api");
    });
});

var configuredConnectionString = builder.Configuration.GetConnectionString("HaiaDatabase");
var connectionString = !string.IsNullOrWhiteSpace(configuredConnectionString)
    ? configuredConnectionString
    : builder.Configuration["HAIA_DB_CONNECTION_STRING"];

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Missing configuration key: ConnectionStrings:HaiaDatabase or HAIA_DB_CONNECTION_STRING");
}

builder.Services.AddHaiaDatabasePersistence(connectionString);
builder.Services.AddHaiaOpenAiIntegration(builder.Configuration);
builder.Services.AddSingleton<IStudioBootstrapConsole, StudioBootstrapConsole>();
builder.Services.AddScoped<IStudioIdentityStore, EfStudioIdentityStore>();
builder.Services.AddScoped<IStudioAuthenticationService, StudioAuthenticationService>();
builder.Services.AddScoped<IStudioAdminBootstrapService, StudioAdminBootstrapService>();
builder.Services.AddScoped<IMemeIntakeStore, EfMemeIntakeStore>();
builder.Services.AddScoped<IMediaObjectStorage, R2MediaObjectStorage>();
builder.Services.AddScoped<IMemeEvaluationService, MemeEvaluationService>();
builder.Services.AddScoped<IMemeIntakeService, MemeIntakeService>();
builder.Services.AddSingleton(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<MemeIntakeOptions>>().Value);
builder.Services.AddSingleton(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<R2StorageOptions>>().Value);

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy("studio-cors", policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS")
                .WithHeaders("Content-Type", "X-Correlation-ID", "X-CSRF-TOKEN", "Accept")
                .AllowCredentials();
        }
    });
});

builder.Services.AddRateLimiter(options =>
{
    var limits = builder.Configuration.GetSection(StudioRateLimitOptions.SectionName).Get<StudioRateLimitOptions>()
        ?? new StudioRateLimitOptions();

    options.AddPolicy("studio-default", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = limits.StudioDefaultPermitLimit,
                Window = TimeSpan.FromSeconds(limits.WindowSeconds),
                QueueLimit = 0,
                AutoReplenishment = true,
            }));

    options.AddPolicy("studio-login", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = limits.StudioLoginPermitLimit,
                Window = TimeSpan.FromSeconds(limits.LoginWindowSeconds),
                QueueLimit = 0,
                AutoReplenishment = true,
            }));
});

var app = builder.Build();

if (shouldBootstrapStudioAdmin)
{
    using var scope = app.Services.CreateScope();
    var bootstrap = scope.ServiceProvider.GetRequiredService<IStudioAdminBootstrapService>();
    await bootstrap.ExecuteAsync(CancellationToken.None);
    return;
}

app.UseExceptionHandler();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseRateLimiter();
app.UseCors("studio-cors");
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "swagger";
        options.SwaggerEndpoint("/openapi/v1.json", "HAIA Studio API v1");
    });
}

app.MapGet("/health/live", () => Results.Ok(new { status = "live" }));
app.MapGet("/health/ready", async (HaiaDbContext db, CancellationToken ct) =>
{
    var canConnect = await db.Database.CanConnectAsync(ct);
    return canConnect ? Results.Ok(new { status = "ready" }) : Results.Problem(statusCode: 503, title: "Database unavailable");
});

app.MapControllers();

app.Run();

public partial class Program;
