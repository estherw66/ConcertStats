using ConcertStats.Application.Interfaces.Services;
using ConcertStats.Application.Services;
using ConcertStats.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ConcertStats.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
            .AddScoped<IEncryptionService, EncryptionService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IPasswordHasher<UserCredentials>, PasswordHasher<UserCredentials>>();
    }
}