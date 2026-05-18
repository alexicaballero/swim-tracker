using SwimTracker.Application.Abstractions.Data;
using SwimTracker.Application.Abstractions.Validation;
using SwimTracker.Application.Clubs;
using SwimTracker.Domain;
using SwimTracker.SharedKernel;

namespace SwimTracker.Application.Swimmers.CreateSwimmer;

public class CreateSwimmerHandler : IRequestHandler<CreateSwimmerRequest, CreateSwimmerResponse>
{
    private readonly IClubRepository _clubRepository;
    private readonly ISwimmerRepository _swimmerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateSwimmerRequest> _validator;

    public CreateSwimmerHandler(
        IClubRepository clubRepository,
        ISwimmerRepository swimmerRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateSwimmerRequest> validator)
    {
        _clubRepository = clubRepository;
        _swimmerRepository = swimmerRepository;
        _unitOfWork = unitOfWork;
        _validator = validator;
    }

    public async Task<Result<CreateSwimmerResponse>> HandleAsync(CreateSwimmerRequest request, CancellationToken cancellationToken)
    {
        var validationErrors = _validator.ValidateRequest(request);
        if (validationErrors.Any())
        {
            return Result.Failure<CreateSwimmerResponse>(
                new Error("Swimmer.ValidationFailed", string.Join("; ", validationErrors)));
        }

        var club = await _clubRepository.GetByIdAsync(request.ClubId, cancellationToken);

        if (club is null)
            return Result.Failure<CreateSwimmerResponse>(ClubErrors.NotFound);

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
            _swimmerRepository.Add(swimmer);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex) when (!(ex is OperationCanceledException))
        {
            throw new ApplicationException("An error occurred while creating the swimmer.", ex);
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