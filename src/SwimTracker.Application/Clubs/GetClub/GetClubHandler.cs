using SwimTracker.Application.Abstractions.Data;
using SwimTracker.Domain;
using SwimTracker.SharedKernel;

namespace SwimTracker.Application.Clubs.GetClub;

public sealed class GetClubHandler : IRequestHandler<GetClubRequest, GetClubResponse>
{
    private readonly IClubRepository _clubRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GetClubHandler(IClubRepository clubRepository, IUnitOfWork unitOfWork)
    {
        _clubRepository = clubRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetClubResponse>> HandleAsync(GetClubRequest request, CancellationToken cancellationToken)
    {
        var club = await _clubRepository.GetByIdAsync(request.Id, cancellationToken);

        if (club is null)
        {
            return Result.Failure<GetClubResponse>(ClubErrors.NotFound);
        }

        var response = new GetClubResponse(club.Id, club.Name, club.Acronym, club.CountryCode, club.City, club.Address, club.Phone, club.Email, club.FederationMemberId, club.LogoUrl);

        return Result.Success(response);
    }
}