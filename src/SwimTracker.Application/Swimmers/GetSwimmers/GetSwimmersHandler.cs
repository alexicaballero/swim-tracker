using SwimTracker.SharedKernel;

namespace SwimTracker.Application.Swimmers.GetSwimmers;

public class GetSwimmersHandler : IHandler<List<GetSwimmersResponse>>
{
    private readonly ISwimmerRepository _swimmerRepository;

    public GetSwimmersHandler(ISwimmerRepository swimmerRepository)
    {
        _swimmerRepository = swimmerRepository;
    }

    public async Task<Result<List<GetSwimmersResponse>>> HandleAsync(CancellationToken cancellationToken)
    {
        var swimmers = await _swimmerRepository.GetAllAsync(cancellationToken);
        var response = swimmers.Select(s => new GetSwimmersResponse(s.Id, 
                                                                    s.ClubId, 
                                                                    s.FirstName, 
                                                                    s.LastName, 
                                                                    s.DateOfBirth, 
                                                                    s.Gender, 
                                                                    s.Nationality, 
                                                                    s.Email, 
                                                                    s.Phone, 
                                                                    s.LicenseNumber, 
                                                                    s.LicenseStatus, 
                                                                    s.LicenseExpiresAt)).ToList();

        return Result.Success(response);
    }
}