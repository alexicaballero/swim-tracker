using SwimTracker.Domain;
using SwimTracker.SharedKernel;

namespace SwimTracker.Application.Swimmers.GetSwimmer;

public class GetSwimmerHandler : IRequestHandler<GetSwimmerRequest, GetSwimmerResponse>
{
    private readonly ISwimmerRepository _swimmerRepository;

    public GetSwimmerHandler(ISwimmerRepository swimmerRepository)
    {
        _swimmerRepository = swimmerRepository;
    }

    public async Task<Result<GetSwimmerResponse>> HandleAsync(GetSwimmerRequest request, CancellationToken cancellationToken)
    {
        var swimmer = await _swimmerRepository.GetByIdAsync(request.Id, cancellationToken);

        if (swimmer == null)
        {
            return Result.Failure<GetSwimmerResponse>(SwimmerErrors.NotFound);
        }

        var response = new GetSwimmerResponse(
            swimmer.Id,
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