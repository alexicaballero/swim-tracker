namespace SwimTracker.SharedKernel;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}