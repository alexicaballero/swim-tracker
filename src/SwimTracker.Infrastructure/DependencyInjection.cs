using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SwimTracker.Application.Abstractions.Data;
using SwimTracker.Application.Clubs;
using SwimTracker.Application.Swimmers;
using SwimTracker.Infrastructure.Persistence;
using SwimTracker.Infrastructure.Persistence.Repositories;
using SwimTracker.Infrastructure.Time;
using SwimTracker.SharedKernel;

namespace SwimTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        AddPersistency(services, configuration);

        return services;
    }

    private static IServiceCollection AddPersistency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SwimTrackerDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DbConnectionString") ?? throw new InvalidOperationException("Connection string 'DbConnectionString' not found.");

            options.UseNpgsql(connectionString, npSqlOptions =>
            {
                npSqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null);
            });
        });

        services.AddScoped<IUnitOfWork>(provider => provider.GetRequiredService<SwimTrackerDbContext>());
        services.AddScoped<IClubRepository, ClubRepository>();
        services.AddScoped<ISwimmerRepository, SwimmerRepository>();

        return services;
    }
}