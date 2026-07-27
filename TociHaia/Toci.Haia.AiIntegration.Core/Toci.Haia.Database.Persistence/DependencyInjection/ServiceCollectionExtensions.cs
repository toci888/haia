using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Toci.Haia.Database.Persistence.Context;

namespace Toci.Haia.Database.Persistence.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHaiaDatabasePersistence(
        this IServiceCollection services,
        string connectionString,
        Action<NpgsqlDbContextOptionsBuilder>? configureNpgsql = null)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ArgumentException("Connection string is required.", nameof(connectionString));
        }

        services.AddDbContext<HaiaDbContext>(options =>
        {
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                configureNpgsql?.Invoke(npgsqlOptions);
            });
        });

        return services;
    }
}
