using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Toci.Haia.Database.Persistence.Context;

namespace Toci.Haia.Database.Persistence.DesignTime;

public sealed class HaiaDbContextFactory : IDesignTimeDbContextFactory<HaiaDbContext>
{
    private const string ConnectionStringEnvironmentVariable = "HAIA_DB_CONNECTION_STRING";

    public HaiaDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable(ConnectionStringEnvironmentVariable);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                $"Missing required environment variable: {ConnectionStringEnvironmentVariable}. Set it before running EF Core design-time commands.");
        }

        var optionsBuilder = new DbContextOptionsBuilder<HaiaDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new HaiaDbContext(optionsBuilder.Options);
    }
}
