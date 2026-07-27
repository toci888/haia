using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.RateLimiting;
using Toci.Haia.Api.Common.Correlation;
using Toci.Haia.Api.Common.Errors;
using Toci.Haia.Api.Features.LegalDocuments;
using Toci.Haia.Api.Features.Registration.Email;
using Toci.Haia.Api.Features.Registration.Nickname;
using Toci.Haia.Api.Infrastructure.EmailVerification;
using Toci.Haia.Database.Persistence.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddHealthChecks();
builder.Services.AddControllers();

builder.Services
    .AddOptions<EmailRegistrationOptions>()
    .Bind(builder.Configuration.GetSection(EmailRegistrationOptions.SectionName))
    .ValidateDataAnnotations()
    .Validate(o => o.MaximumPasswordLength >= o.MinimumPasswordLength,
        "MaximumPasswordLength must be greater than or equal to MinimumPasswordLength.")
    .ValidateOnStart();

builder.Services
    .AddOptions<EmailRegistrationRateLimitOptions>()
    .Bind(builder.Configuration.GetSection(EmailRegistrationRateLimitOptions.SectionName))
    .ValidateOnStart();

var configuredConnectionString = builder.Configuration.GetConnectionString("HaiaDatabase");
var connectionString = !string.IsNullOrWhiteSpace(configuredConnectionString)
    ? configuredConnectionString
    : builder.Configuration["HAIA_DB_CONNECTION_STRING"];

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Missing configuration key: ConnectionStrings:HaiaDatabase or HAIA_DB_CONNECTION_STRING");
}

builder.Services.AddHaiaDatabasePersistence(connectionString);

builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);
builder.Services.AddScoped<IEmailRegistrationStore, EfEmailRegistrationStore>();
builder.Services.AddScoped<ILegalDocumentsService, EfLegalDocumentsService>();
builder.Services.AddScoped<INicknameAvailabilityService, EfNicknameAvailabilityService>();
builder.Services.AddScoped<IEmailRegistrationService, EmailRegistrationService>();

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddSingleton<InMemoryEmailVerificationSender>();
    builder.Services.AddSingleton<IEmailVerificationSender>(sp => sp.GetRequiredService<InMemoryEmailVerificationSender>());
}

builder.Services.AddRateLimiter(options =>
{
    var limits = builder.Configuration.GetSection(EmailRegistrationRateLimitOptions.SectionName).Get<EmailRegistrationRateLimitOptions>()
        ?? new EmailRegistrationRateLimitOptions();

    static FixedWindowRateLimiterOptions Create(int permitLimit, int windowSeconds) => new()
    {
        PermitLimit = permitLimit,
        Window = TimeSpan.FromSeconds(windowSeconds),
        QueueLimit = 0,
        AutoReplenishment = true
    };

    options.AddPolicy("registration", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => Create(limits.RegistrationPermitLimit, limits.WindowSeconds)));

    options.AddPolicy("verification", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => Create(limits.VerificationPermitLimit, limits.WindowSeconds)));

    options.AddPolicy("resend", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => Create(limits.ResendPermitLimit, limits.WindowSeconds)));

    options.AddPolicy("nickname", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => Create(limits.NicknamePermitLimit, limits.WindowSeconds)));
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.RoutePrefix = "swagger";
        options.SwaggerEndpoint("/openapi/v1.json", "HAIA API v1");
    });
}

app.MapGet("/health/live", () => Results.Ok(new { status = "live" }));

app.MapGet("/health/ready", async (Toci.Haia.Database.Persistence.Context.HaiaDbContext db, CancellationToken ct) =>
{
    var canConnect = await db.Database.CanConnectAsync(ct);
    return canConnect ? Results.Ok(new { status = "ready" }) : Results.Problem(statusCode: 503, title: "Database unavailable");
});

app.MapControllers();

app.Run();

public partial class Program;
