using FastEndpoints;
using SwimTracker.Application.Clubs.CreateClub;

namespace SwimTracker.Api.REPR.Endpoints.Clubs;

/// <summary>
/// Endpoint for creating a new club.
/// </summary>
public class CreateClub : Endpoint<CreateClubRequest>
{
    private readonly IRequestHandler<CreateClubRequest> _requestHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateClub"/> class.
    /// </summary>
    /// <param name="requestHandler">The request handler for processing the create club request.</param>
    public CreateClub(IRequestHandler<CreateClubRequest> requestHandler)
    {
        _requestHandler = requestHandler;
    }

    /// <summary>
    /// Configures the endpoint.
    /// </summary>
    public override void Configure()
    {
        Post("/clubs");
        AllowAnonymous();
    }

    /// <summary>
    /// Handles the incoming request to create a new club.
    /// </summary>
    /// <param name="req">The request containing the club details.</param>
    /// <param name="ct">The cancellation token.</param>
    public override async Task HandleAsync(CreateClubRequest req, CancellationToken ct)
    {
        var result = await _requestHandler.HandleAsync(req, ct);

        if (result.IsSuccess)
        {
            await Send.CreatedAtAsync<CreateClub>($"/api/clubs/{req.Name}", req, cancellation: ct);
        }
        else
        {
            await Send.StatusCodeAsync(statusCode: 500, ct);
        }
    }
}