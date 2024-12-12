using DevTask.CreditProcessor.Data.Repositories;
using DevTask.CreditProcessor.Domain.Abstractions;

using Microsoft.Data.Sqlite;

using System.Data;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyConfiguration
{
    public static IServiceCollection AddDataAccess(this IServiceCollection services, string connectionString)
    {
        services
            .AddSQLiteDb(connectionString)
            .AddRepositories();

        return services;
    }

    private static IServiceCollection AddSQLiteDb(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IDbConnection>(_ => new SqliteConnection(connectionString));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICreditRepository, CreditRepository>();

        return services;
    }
}