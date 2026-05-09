namespace SwimTracker.Application.Swimmers.CreateSwimmer;

public sealed record CreateSwimmerResponse(
    Guid ClubId,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string Gender,
    string Nationality,
    string? Email,
    string? Phone,
    string? LicenseNumber,
    string? LicenseStatus,
    DateOnly? LicenseExpiresAt);