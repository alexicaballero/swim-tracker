namespace SwimTracker.SharedKernel;

public abstract class AuditableEntity : Entity, IAuditable
{
    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? UpdatedAt { get; set; }
}