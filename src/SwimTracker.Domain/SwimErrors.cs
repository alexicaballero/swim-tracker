using SwimTracker.SharedKernel;

namespace SwimTracker.Domain;

public static class SwimmerErrors
{
    public static readonly Error NotFound = new("Swimmer.NotFound", "The specified swimmer was not found.");
    public static readonly Error InvalidName = new("Swimmer.InvalidName", "Swimmer name is required.");
    public static readonly Error InvalidCity = new("Swimmer.InvalidCity", "City is required.");
    public static readonly Error InvalidEmail = new("Swimmer.InvalidEmail", "Email is required.");
    public static readonly Error CreationFailed = new("Swimmer.CreationFailed", "Failed to create swimmer.");
}