using SwimTracker.SharedKernel;

namespace SwimTracker.Infrastructure.Time;


internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
