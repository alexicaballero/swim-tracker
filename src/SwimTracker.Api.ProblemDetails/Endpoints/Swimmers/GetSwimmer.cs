using SwimTracker.Api.ProblemDetails.Endpoints;
using SwimTracker.Application.Swimmers.GetSwimmer;

namespace SwimTracker.Api.ProblemDetails.Endpoints.Swimmers;

/// <summary>
/// Endpoint for retrieving a swimmer by ID.
/// </summary>
public class GetSwimmer : IEndpoint
{
    /// <summary>
    /// Maps the GET endpoint for retrieving a swimmer.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/swimmers/{id:guid}", HandleAsync)
            .WithTags("Swimmers");
    }

    /// <summary>
    /// Handles the GET request for a swimmer by ID.
    /// </summary>
    /// <param name="id">The swimmer's unique identifier.</param>
    /// <param name="requestHandler">The request handler for GetSwimmer.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task<IResult> HandleAsync(
        Guid id,
        IRequestHandler<GetSwimmerRequest, GetSwimmerResponse> requestHandler,
        CancellationToken cancellationToken)
    {
        var request = new GetSwimmerRequest(id);
        var result = await requestHandler.HandleAsync(request, cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return Results.Problem(new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = result.Error.Code,
            Title = "Swimmer not found",
            Detail = result.Error.Description,
            Status = StatusCodes.Status404NotFound
        });
    }
}