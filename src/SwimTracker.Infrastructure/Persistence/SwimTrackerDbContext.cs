using Microsoft.EntityFrameworkCore;
using SwimTracker.Application.Abstractions.Data;
using SwimTracker.Domain;

namespace SwimTracker.Infrastructure.Persistence;

public sealed class SwimTrackerDbContext : DbContext, IUnitOfWork
{
    public DbSet<Club> Clubs => Set<Club>();
    public DbSet<Swimmer> Swimmers => Set<Swimmer>();


    public SwimTrackerDbContext(DbContextOptions<SwimTrackerDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Apply all configurations from the assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SwimTrackerDbContext).Assembly);

        {

        }
    }
}
