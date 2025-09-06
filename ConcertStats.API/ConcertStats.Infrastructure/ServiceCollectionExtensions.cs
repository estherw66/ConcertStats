using ConcertStats.Application.Interfaces.Repositories;
using ConcertStats.Infrastructure.Persistence;
using ConcertStats.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ConcertStats.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        return services
            .AddConcertStatsDatabase(connectionString)
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IArtistRepository, ArtistRepository>()
            .AddScoped<IVenueRepository, VenueRepository>();
    }
}