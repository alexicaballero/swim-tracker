namespace SwimTracker.Application.Swimmers.GetSwimmers;

public record GetSwimmersResponse(
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