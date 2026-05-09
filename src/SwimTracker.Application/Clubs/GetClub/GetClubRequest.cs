using SwimTracker.Application.Abstractions.Messaging;

namespace SwimTracker.Application.Clubs.GetClub;

public sealed record GetClubRequest(Guid id) : IRequest<ClubResponse>;