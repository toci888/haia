using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Toci.Haia.Database.Persistence.Context;
using Toci.Haia.Studio.Api.Features.Authentication;
using Toci.Haia.Studio.Api.Infrastructure;

namespace Toci.Haia.Studio.Api.Tests.Unit;

public class StudioAdminBootstrapServiceTests
{
    [Fact]
    public async Task Bootstrap_ShouldBeIdempotent()
    {
        var dbOptions = new DbContextOptionsBuilder<HaiaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;

        await using var db = new HaiaDbContext(dbOptions);
        var firstConsole = new TestBootstrapConsole("admin@haia.local", "Password123!@");
        var firstRunService = new StudioAdminBootstrapService(
            db,
            TimeProvider.System,
            Options.Create(new StudioPasswordPolicyOptions()),
            firstConsole,
            NullLogger<StudioAdminBootstrapService>.Instance);

        await firstRunService.ExecuteAsync(CancellationToken.None);

        var secondConsole = new TestBootstrapConsole("admin@haia.local", "Password123!@");
        var secondRunService = new StudioAdminBootstrapService(
            db,
            TimeProvider.System,
            Options.Create(new StudioPasswordPolicyOptions()),
            secondConsole,
            NullLogger<StudioAdminBootstrapService>.Instance);

        await secondRunService.ExecuteAsync(CancellationToken.None);

        Assert.Equal(1, db.AccountEmails.Count(x => x.EmailNormalized == "admin@haia.local" && x.ReleasedAt == null));
        Assert.Equal(1, db.PasswordCredentials.Count(x => x.IsActive));
        Assert.Equal(1, db.AccountRoles.Count(x => x.RoleKey == "StudioAdmin" && x.RoleScope == StudioScopeMapping.StudioRoleScope));
        Assert.Equal(1, db.AccountRoleAssignments.Count(x => x.IsActive));
    }

    [Fact]
    public async Task Bootstrap_ShouldNotPrintPassword()
    {
        var dbOptions = new DbContextOptionsBuilder<HaiaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;

        await using var db = new HaiaDbContext(dbOptions);
        var console = new TestBootstrapConsole("admin@haia.local", "Password123!@");
        var service = new StudioAdminBootstrapService(
            db,
            TimeProvider.System,
            Options.Create(new StudioPasswordPolicyOptions()),
            console,
            NullLogger<StudioAdminBootstrapService>.Instance);

        await service.ExecuteAsync(CancellationToken.None);

        Assert.DoesNotContain("Password123!@", console.Output, StringComparison.Ordinal);
    }

    private sealed class TestBootstrapConsole(string email, string password) : IStudioBootstrapConsole
    {
        private bool _emailRead;
        private bool _passwordRead;

        public string Output { get; private set; } = string.Empty;

        public string? ReadLine()
        {
            if (_emailRead)
            {
                return null;
            }

            _emailRead = true;
            return email;
        }

        public string ReadSecret()
        {
            if (_passwordRead)
            {
                return string.Empty;
            }

            _passwordRead = true;
            return password;
        }

        public void Write(string text)
        {
            Output += text;
        }

        public void WriteLine(string text)
        {
            Output += text + Environment.NewLine;
        }
    }
}
