using SwimTracker.Application.Clubs.GetClub;

namespace SwimTracker.Api.Endpoints.Clubs;

/// <summary>
/// Endpoint for retrieving a club by its ID.
/// </summary>
public class GetClub : IEndpoint
{
    /// <summary>
    /// Maps the endpoint for retrieving a club by its ID.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/clubs/{id:guid}", HandleAsync)
            .WithTags("Clubs");
    }

    /// <summary>
    /// Handles the GET request to retrieve a club by its ID.
    /// </summary>
    /// <param name="id">The ID of the club to retrieve.</param>
    /// <param name="requestHandler">The handler for processing the request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the club retrieval.</returns>
    private static async Task<IResult> HandleAsync(Guid id, IRequestHandler<GetClubRequest, GetClubResponse> requestHandler, CancellationToken cancellationToken)
    {
        var request = new GetClubRequest(id);
        var result = await requestHandler.HandleAsync(request, cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }
        else
        {
            return Results.NotFound();
        }
    }
}