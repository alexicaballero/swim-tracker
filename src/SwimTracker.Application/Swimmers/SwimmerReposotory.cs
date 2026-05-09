using SwimTracker.Domain;

namespace SwimTracker.Application.Swimmers;

public interface ISwimmerRepository
{
    Task<Swimmer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Swimmer>> GetAllAsync(CancellationToken cancellationToken = default);

    void AddAsync(Swimmer swimmer);

    void UpdateAsync(Swimmer swimmer);

    void DeleteAsync(Swimmer swimmer);
}