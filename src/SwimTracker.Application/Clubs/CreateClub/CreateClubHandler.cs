using SwimTracker.Application.Abstractions.Data;
using SwimTracker.Domain;
using SwimTracker.SharedKernel;

namespace SwimTracker.Application.Clubs.CreateClub;

public class CreateClubHandler : IRequestHandler<CreateClubRequest>
{
    private readonly IClubRepository _clubRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateClubHandler(IClubRepository clubRepository, IUnitOfWork unitOfWork)
    {
        _clubRepository = clubRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateClubRequest request, CancellationToken cancellationToken)
    {
        // Validate required fields
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result.Failure(ClubErrors.InvalidName);

        if (string.IsNullOrWhiteSpace(request.CountryCode))
            return Result.Failure(ClubErrors.InvalidCountryCode);

        if (string.IsNullOrWhiteSpace(request.City))
            return Result.Failure(ClubErrors.InvalidCity);

        if (string.IsNullOrWhiteSpace(request.Email))
            return Result.Failure(ClubErrors.InvalidEmail);

        var club = Club.Create(
             name: request.Name,
             acronym: request.Acronym,
             countryCode: request.CountryCode,
             city: request.City,
             email: request.Email
        );

        _clubRepository.Add(club);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}