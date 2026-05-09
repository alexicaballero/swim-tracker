namespace SwimTracker.Application.Clubs.GetClub;

public sealed record ClubResponse(
    Guid Id,
    string Name,
    string Acronym,
    string CountryCode,
    string City,
    string? Address,
    string? Phone,
    string Email,
    string? FederationMemberId,
    string? LogoUrl);