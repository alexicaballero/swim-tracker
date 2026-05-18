using Microsoft.EntityFrameworkCore;
using SwimTracker.Application.Swimmers;
using SwimTracker.Domain;
using SwimTracker.Infrastructure.Persistence;

namespace SwimTracker.Infrastructure.Persistence.Repositories;

internal sealed class SwimmerRepository : ISwimmerRepository
{
    private readonly SwimTrackerDbContext _dbContext;

    public SwimmerRepository(SwimTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public Task<Swimmer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.Swimmers.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }


    public Task<List<Swimmer>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Swimmers.ToListAsync(cancellationToken);
    }

    public void Add(Swimmer swimmer)
    {
        _dbContext.Swimmers.Add(swimmer);
    }

    public void Delete(Swimmer swimmer)
    {
        _dbContext.Swimmers.Remove(swimmer);
    }

    public void Update(Swimmer swimmer)
    {
        _dbContext.Swimmers.Update(swimmer);
    }
}