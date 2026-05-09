using SwimTracker.Application.Abstractions.Data;
using SwimTracker.Domain;
using SwimTracker.SharedKernel;

namespace SwimTracker.Application.Swimmers.CreateSwimmer;

public class CreateSwimmerHandler : IRequestHandler<CreateSwimmerRequest, CreateSwimmerResponse>
{
    private readonly ISwimmerRepository _swimmerRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSwimmerHandler(ISwimmerRepository swimmerRepository, IUnitOfWork unitOfWork)
    {
        _swimmerRepository = swimmerRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateSwimmerResponse>> HandleAsync(CreateSwimmerRequest request, CancellationToken cancellationToken)
    {
        // TODO: implement validation.

        var swimmer = Swimmer.Create(
            request.ClubId,
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.Gender,
            request.Nationality,
            request.Email,
            request.Phone,
            request.LicenseNumber,
            request.LicenseStatus,
            request.LicenseExpiresAt,
            Guid.Parse("00000000-0000-0000-0000-000000000001")
            );

        try
        {
            _swimmerRepository.AddAsync(swimmer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
            return Result.Failure<CreateSwimmerResponse>(SwimmerErrors.CreationFailed);
        }

        var response = new CreateSwimmerResponse(
        swimmer.ClubId,
        swimmer.FirstName,
        swimmer.LastName,
        swimmer.DateOfBirth,
        swimmer.Gender,
        swimmer.Nationality,
        swimmer.Email,
        swimmer.Phone,
        swimmer.LicenseNumber,
        swimmer.LicenseStatus,
        swimmer.LicenseExpiresAt);

        return Result.Success(response);
    }
}