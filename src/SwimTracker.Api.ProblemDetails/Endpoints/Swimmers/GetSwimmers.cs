using SwimTracker.Application.Swimmers.GetSwimmers;

namespace SwimTracker.Api.ProblemDetails.Endpoints.Swimmers;

/// <summary>
/// Endpoint for retrieving a list of swimmers.
/// </summary>
public class GetSwimmers : IEndpoint
{
    /// <summary>
    /// Maps the GET swimmers endpoint to the route builder.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/swimmers", HandleAsync)
            .WithTags("Swimmers");
    }

    /// <summary>
    /// Handles the GET request for all swimmers.
    /// </summary>
    /// <param name="requestHandler">The handler for the request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the operation as an IResult.</returns>
    private async Task<IResult> HandleAsync(
        IHandler<List<GetSwimmersResponse>> requestHandler,
        CancellationToken cancellationToken)
    {
        var result = await requestHandler.HandleAsync(cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return Results.Problem(new Microsoft.AspNetCore.Mvc.ProblemDetails
        {
            Type = result.Error.Code,
            Title = "Failed to retrieve swimmers",
            Detail = result.Error.Description,
            Status = StatusCodes.Status500InternalServerError
        });
    }
}