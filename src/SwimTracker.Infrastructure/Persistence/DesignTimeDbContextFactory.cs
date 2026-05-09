using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SwimTracker.Infrastructure.Persistence;


/// <summary>Creates <see cref="SwimTrackerDbContext"/> instances for EF Core design-time tooling.</summary>
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<SwimTrackerDbContext>
{
    /// <summary>Creates a configured <see cref="SwimTrackerDbContext"/> for migrations and other tooling.</summary>
    public SwimTrackerDbContext CreateDbContext(string[] args)
    {
        const string connectionString = "Host=localhost;Port=5432;Database=swimtracker;Username=swim;Password=swim_secret";

        var options = SwimTrackerDbContextOptions.Configure(
            new DbContextOptionsBuilder<SwimTrackerDbContext>(),
            connectionString)
        .Options;

        return new SwimTrackerDbContext(options);
    }

    //private static IConfiguration BuildConfiguration()
    //{
    //    var apiProjectPath = Path.GetFullPath(
    //        Path.Combine(Directory.GetCurrentDirectory(), "..", "SwimTracker.API"));

    //    return new ConfigurationBuilder()
    //        .SetBasePath(apiProjectPath)
    //        .AddJsonFile("appsettings.json", optional: true)
    //        .AddJsonFile("appsettings.Development.json", optional: true)
    //        .AddEnvironmentVariables()
    //        .Build();
    //}
}
