using FastEndpoints;
using SwimTracker.Application.Clubs.GetClub;

namespace SwimTracker.Api.REPR.Endpoints.Clubs;

/// <summary>
/// Endpoint for retrieving a club by its ID.
/// </summary>
public class GetClub : Endpoint<GetClubRequest, ClubResponse>
{
    private readonly IRequestHandler<GetClubRequest, ClubResponse> _requestHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetClub"/> class.
    /// </summary>
    /// <param name="requestHandler">The request handler for processing the get club request.</param>
    public GetClub(IRequestHandler<GetClubRequest, ClubResponse> requestHandler)
    {
        _requestHandler = requestHandler;
    }

    /// <summary>
    /// Configures the endpoint.
    /// </summary>
    public override void Configure()
    {
        Get("/clubs/{id:guid}");
        AllowAnonymous();
    }

    /// <summary>
    /// Handles the incoming request to retrieve a club by its ID.
    /// </summary>
    /// <param name="req">The request containing the club ID.</param>
    /// <param name="ct">The cancellation token.</param>
    public override async Task HandleAsync(GetClubRequest req, CancellationToken ct)
    {
        var result = await _requestHandler.HandleAsync(req, ct);

        if (result.IsSuccess)
        {
            await Send.OkAsync(result.Value, ct);
        }
        else
        {
            await Send.NotFoundAsync(ct);
        }
    }
}