using Microsoft.AspNetCore.Mvc;
using SwimTracker.Api.ProblemDetails.Endpoints;
using SwimTracker.Application.Swimmers.CreateSwimmer;

namespace SwimTracker.Api.ProblemDetails.Endpoints.Swimmers;

/// <summary>
/// Endpoint for creating a new swimmer.
/// </summary>
public class CreateSwimmer : IEndpoint
{
    /// <summary>
    /// Maps the endpoint for creating a swimmer.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/swimmers", HandleAsync)
        .WithTags("Swimmers");
    }

    /// <summary>
    /// Handles the HTTP POST request to create a swimmer.
    /// </summary>
    /// <param name="request">The create swimmer request.</param>
    /// <param name="requestHandler">The request handler for creating a swimmer.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the operation.</returns>
    private async Task<IResult> HandleAsync(
        [FromBody] CreateSwimmerRequest request,
        IRequestHandler<CreateSwimmerRequest, CreateSwimmerResponse> requestHandler,
        CancellationToken cancellationToken)
    {
        var result = await requestHandler.HandleAsync(request, cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return Results.Problem(new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = result.Error.Code,
            Title = "Swimmer creation failed",
            Detail = result.Error.Description,
            Status = StatusCodes.Status400BadRequest
        });
    }
}