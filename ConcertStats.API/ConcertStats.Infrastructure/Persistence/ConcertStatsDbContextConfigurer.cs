using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ConcertStats.Infrastructure.Persistence;

public static class ConcertStatsDbContextConfigurer
{
    public static IServiceCollection AddConcertStatsDatabase(this IServiceCollection services, string connectionString)
    {
        return services
            .AddConcertStatsDbContext(connectionString);
    }

    private static IServiceCollection AddConcertStatsDbContext(this IServiceCollection services,
        string connectionString)
    {
        return services
            .AddDbContext<ConcertStatsDbContext>((_, options) =>
            {
                options
                    .UseMySql(
                        connectionString,
                        ServerVersion.AutoDetect(connectionString));
            });
    }
}