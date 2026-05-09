namespace SwimTracker.SharedKernel;

/// <summary>
/// Defines properties for tracking the creation and last update timestamps of an auditable entity.
/// </summary>
/// <remarks>Implement this interface to provide standardized audit information for entities, such as when they
/// were created and last modified. All timestamps are represented in Coordinated Universal Time (UTC).</remarks>
public interface IAuditable
{
    /// <summary>
    /// Gets the UTC timestamp when the entity was created.
    /// </summary>
    DateTimeOffset CreatedAt { get; }

    /// <summary>
    /// Gets the UTC timestamp of the last entity update.
    /// </summary>
    DateTimeOffset? UpdatedAt { get; }
}