using Microsoft.EntityFrameworkCore;

namespace SwimTracker.Infrastructure.Persistence;

/// <summary>
/// Provides shared EF Core options configuration for <see cref="SwimTrackerDbContext"/>.
/// </summary>
internal static class SwimTrackerDbContextOptions
{
    /// <summary>
    /// Configures the database provider for <see cref="SwimTrackerDbContext"/>.
    /// </summary>
    public static DbContextOptionsBuilder Configure(DbContextOptionsBuilder builder, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        builder.UseNpgsql(connectionString, npSqlOptions =>
        {
            npSqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorCodesToAdd: null);
        });

        return builder;
    }

    /// <summary>
    /// Configures the database provider for <see cref="SwimTrackerDbContext"/>.
    /// </summary>
    public static DbContextOptionsBuilder<SwimTrackerDbContext> Configure(
        DbContextOptionsBuilder<SwimTrackerDbContext> builder,
        string connectionString) =>
        (DbContextOptionsBuilder<SwimTrackerDbContext>)Configure((DbContextOptionsBuilder)builder, connectionString);
}