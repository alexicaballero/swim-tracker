using SwimTracker.Domain;

namespace SwimTracker.Application.Swimmers;

public interface ISwimmerRepository
{
    Task<Swimmer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<List<Swimmer>> GetAllAsync(CancellationToken cancellationToken = default);

    void Add(Swimmer swimmer);

    void Update(Swimmer swimmer);

    void Delete(Swimmer swimmer);
}