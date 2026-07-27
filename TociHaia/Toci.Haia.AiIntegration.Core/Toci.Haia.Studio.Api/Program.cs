using System.Threading.RateLimiting;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Toci.Haia.Database.Persistence.Context;
using Toci.Haia.Database.Persistence.DependencyInjection;
using Toci.Haia.Studio.Api.Common.Correlation;
using Toci.Haia.Studio.Api.Common.Errors;
using Toci.Haia.Studio.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

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
    .AddOptions<StudioAuthOptions>()
    .Bind(builder.Configuration.GetSection(StudioAuthOptions.SectionName))
    .ValidateOnStart();

var studioAuth = builder.Configuration.GetSection(StudioAuthOptions.SectionName).Get<StudioAuthOptions>()
    ?? new StudioAuthOptions();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;

        if (!string.IsNullOrWhiteSpace(studioAuth.Authority))
        {
            options.Authority = studioAuth.Authority;
            options.Audience = studioAuth.Audience;
        }
        else
        {
            var key = builder.Configuration["HAIA_STUDIO_AUTH_SIGNING_KEY"];
            if (string.IsNullOrWhiteSpace(key))
            {
                key = studioAuth.SigningKey;
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException("Missing configuration key: StudioAuth:SigningKey or HAIA_STUDIO_AUTH_SIGNING_KEY");
            }

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = studioAuth.Issuer,
                ValidateAudience = true,
                ValidAudience = studioAuth.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1),
            };
        }
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("studio-access", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireRole(studioAuth.RequiredRole);
        policy.RequireClaim("scope", studioAuth.RequiredScope);
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

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
builder.Services.AddCors(options =>
{
    options.AddPolicy("studio-cors", policy =>
    {
        if (allowedOrigins.Length > 0)
        {
            policy.WithOrigins(allowedOrigins)
                .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS")
                .WithHeaders("Content-Type", "X-Correlation-ID", "Accept");
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
});

var app = builder.Build();

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
