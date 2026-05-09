namespace SwimTracker.Application.Clubs.GetClubs;

public record GetClubsResponse(
    Guid Id,
    string Name,
    string Acronym,
    string CountryCode,
    string City,
    string? Address,
    string? Phone,
    string Email,
    string? FederationMemberId,
    string? LogoUrl
);