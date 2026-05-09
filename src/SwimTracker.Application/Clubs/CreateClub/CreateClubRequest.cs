using SwimTracker.Application.Abstractions.Messaging;

namespace SwimTracker.Application.Clubs.CreateClub;

public record CreateClubRequest(
    string Name,
    string Acronym,
    string CountryCode,
    string City,
    string Email) : IRequest;