using SwimTracker.SharedKernel;

namespace SwimTracker.Domain;

public static class ClubErrors
{
    public static readonly Error NotFound = new("Club.NotFound", "The specified club was not found.");
    public static readonly Error InvalidName = new("Club.InvalidName", "Club name is required.");
    public static readonly Error InvalidCountryCode = new("Club.InvalidCountryCode", "Country code is required.");
    public static readonly Error InvalidCity = new("Club.InvalidCity", "City is required.");
    public static readonly Error InvalidEmail = new("Club.InvalidEmail", "Email is required.");
}