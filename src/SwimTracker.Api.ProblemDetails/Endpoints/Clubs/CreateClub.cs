using Microsoft.AspNetCore.Mvc;
using SwimTracker.Api.ProblemDetails.Endpoints;
using SwimTracker.Application.Clubs.CreateClub;

namespace SwimTracker.Api.ProblemDetails.Endpoints.Clubs;

/// <summary>
/// Endpoint for creating a new club.
/// </summary>
public class CreateClub : IEndpoint
{
    /// <summary>
    /// Maps the endpoint for creating a new club.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    /// <summary>
    /// Maps the endpoint for creating a new club.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/clubs", HandleAsync)
            .WithTags("Clubs");
    }

    /// <summary>
    /// Handles the HTTP POST request to create a club.
    /// </summary>
    /// <param name="request">The create club request.</param>
    /// <param name="requestHandler">The request handler for creating a club.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the operation as an IResult.</returns>
    private async Task<IResult> HandleAsync(
        [FromBody] CreateClubRequest request,
        IRequestHandler<CreateClubRequest> requestHandler,
        CancellationToken cancellationToken)
    {
        var result = await requestHandler.HandleAsync(request, cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Created($"api/clubs/{request.Name}", request);
        }

        return Results.Problem(new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = result.Error.Code,
            Title = "Club creation failed",
            Detail = result.Error.Description,
            Status = StatusCodes.Status400BadRequest
        });
    }
}