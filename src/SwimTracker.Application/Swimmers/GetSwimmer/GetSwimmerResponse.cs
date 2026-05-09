namespace SwimTracker.Application.Swimmers.GetSwimmer;

public record GetSwimmerResponse(
    Guid Id,
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