using SwimTracker.Application.Clubs.GetClubs;

namespace SwimTracker.Api.Endpoints.Clubs;

/// <summary>
/// Endpoint for retrieving a list of clubs.
/// </summary>
public class GetClubs : IEndpoint
{
    /// <summary>
    /// Maps the endpoint for retrieving a list of clubs.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/clubs", HandleAsync)
            .WithTags("Clubs");
    }

    /// <summary>
    /// Handles the GET request to retrieve a list of clubs.
    /// </summary>
    /// <param name="requestHandler">The handler for processing the request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the clubs retrieval.</returns>
    private static async Task<IResult> HandleAsync(IHandler<List<GetClubsResponse>> requestHandler, CancellationToken cancellationToken)
    {
        var result = await requestHandler.HandleAsync(cancellationToken);

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