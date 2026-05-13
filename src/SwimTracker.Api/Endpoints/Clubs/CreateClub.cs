using Microsoft.AspNetCore.Mvc;
using SwimTracker.Application.Clubs.CreateClub;

namespace SwimTracker.Api.Endpoints.Clubs;

/// <summary>
/// Endpoint for creating a new club.
/// </summary>
public class CreateClub : IEndpoint
{
    /// <summary>
    /// Maps the endpoint for creating a new club.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        /// <summary>
        /// Handles the POST request to create a new club.
        /// </summary>
        /// <param name="request">The request containing the club details.</param>
        /// <param name="requestHandler">The handler for processing the request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result of the club creation.</returns>
        app.MapPost("api/clubs", async ([FromBody] CreateClubRequest request, IRequestHandler<CreateClubRequest> requestHandler, CancellationToken cancellationToken) =>
        {
            var result = await requestHandler.HandleAsync(request, cancellationToken);

            if (result.IsSuccess)
            {
                return Results.Created($"api/clubs/{request.Name}", request);
            }
            else
            {
                return Results.BadRequest(result.Error);
            }
        })
        .WithTags("Clubs");
    }
}