using Microsoft.EntityFrameworkCore;
using SwimTracker.Application.Clubs;
using SwimTracker.Domain;
using SwimTracker.Infrastructure.Persistence;

namespace SwimTracker.Infrastructure.Persistence.Repositories;

internal sealed class ClubRepository : IClubRepository
{
    private readonly SwimTrackerDbContext _dbContext;

    public ClubRepository(SwimTrackerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Club club)
    {
        _dbContext.Clubs.Add(club);
    }

    public Task<List<Club>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.Clubs.ToListAsync(cancellationToken);
    }

    public async Task<Club?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Clubs.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public void Update(Club club)
    {
        _dbContext.Clubs.Update(club);
    }
}
