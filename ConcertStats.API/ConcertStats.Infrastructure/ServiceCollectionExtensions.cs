using ConcertStats.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace ConcertStats.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        return services
            .AddConcertStatsDatabase(connectionString);
    }
}