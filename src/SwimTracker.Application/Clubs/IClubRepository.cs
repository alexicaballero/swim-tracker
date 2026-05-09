using SwimTracker.Domain;

namespace SwimTracker.Application.Clubs;

/// <summary>
/// Defines the contract for a repository that manages club entities, providing methods to retrieve and update club information.
/// </summary>
public interface IClubRepository
{
    /// <summary>
    /// Retrieves all clubs from the repository.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of clubs.</returns>
    Task<List<Club>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the club with the specified identifier, or null if not found.
    /// </summary>
    /// <param name="id">The unique identifier of the club.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the club if found; otherwise, null.</returns>
    Task<Club?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new club to the repository, persisting it to the underlying data store.
    /// </summary>
    /// <param name="club">The club entity to add.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    void Add(Club club);

    /// <summary>
    /// Persists changes to an existing club.
    /// </summary>
    void Update(Club club);
}