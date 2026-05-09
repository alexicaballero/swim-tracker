using SwimTracker.Application.Abstractions.Data;
using SwimTracker.Domain;
using SwimTracker.SharedKernel;

namespace SwimTracker.Application.Clubs.GetClub;

public sealed class GetClubHandler : IRequestHandler<GetClubRequest, ClubResponse>
{
    private readonly IClubRepository _clubRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetClubHandler(IClubRepository clubRepository, IUnitOfWork unitOfWork)
    {
        _clubRepository = clubRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ClubResponse>> HandleAsync(GetClubRequest request, CancellationToken cancellationToken)
    {
        var club = await _clubRepository.GetByIdAsync(request.id);

        if (club is null)
        {
            return Result.Failure<ClubResponse>(ClubErrors.NotFound);
        }

        var response = new ClubResponse(club.Id, club.Name, club.Acronym, club.CountryCode, club.City, club.Address, club.Phone, club.Email, club.FederationMemberId, club.LogoUrl);

        return Result.Success(response);
    }
}