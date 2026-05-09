using SwimTracker.SharedKernel;

namespace SwimTracker.Application.Clubs.GetClubs;

public partial class GetClubsHandler : IHandler<List<GetClubsResponse>>
{
    private readonly IClubRepository _clubRepository;

    public GetClubsHandler(IClubRepository clubRepository)
    {
        _clubRepository = clubRepository;
    }

    public async Task<Result<List<GetClubsResponse>>> HandleAsync(CancellationToken cancellationToken)
    {
        var clubs = await _clubRepository.GetAllAsync(cancellationToken);

        var response = clubs.Select(c => new GetClubsResponse(c.Id, c.Name, c.Acronym, c.CountryCode, c.City, c.Address, c.Phone, c.Email, c.FederationMemberId, c.LogoUrl)).ToList();

        return Result.Success(response);
    }
}