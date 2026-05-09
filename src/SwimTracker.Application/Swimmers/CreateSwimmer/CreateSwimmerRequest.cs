using SwimTracker.Application.Abstractions.Messaging;

namespace SwimTracker.Application.Swimmers.CreateSwimmer;

public sealed record CreateSwimmerRequest(
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
    DateOnly? LicenseExpiresAt) : IRequest<CreateSwimmerResponse>;