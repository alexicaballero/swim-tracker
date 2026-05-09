using FastEndpoints;
using SwimTracker.Application.Clubs.GetClubs;

namespace SwimTracker.Api.REPR.Endpoints.Clubs;

/// <summary>
/// Endpoint for retrieving a list of all clubs.
/// </summary>
public class GetClubs : EndpointWithoutRequest<List<GetClubsResponse>>
{
    private readonly IHandler<List<GetClubsResponse>> _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetClubs"/> class.
    /// </summary>
    /// <param name="handler">The handler for processing the get clubs request.</param>
    public GetClubs(IHandler<List<GetClubsResponse>> handler)
    {
        _handler = handler;
    }

    /// <summary>
    /// Configures the endpoint.
    /// </summary>
    public override void Configure()
    {
        Get("/clubs");
        Tags("Clubs");
        AllowAnonymous();
    }

    /// <summary>
    /// Handles the incoming request to retrieve a list of all clubs.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _handler.HandleAsync(ct);

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