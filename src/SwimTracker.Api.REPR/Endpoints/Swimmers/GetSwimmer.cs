using SwimTracker.Application.Swimmers.GetSwimmer;

namespace SwimTracker.Api.REPR.Endpoints.Swimmers;

/// <summary>
/// Endpoint for retrieving a swimmer by their unique identifier.
/// </summary>
public class GetSwimmer : IEndpoint
{
    /// <summary>
    /// Maps the endpoint to the specified route builder.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/swimmers/{id:guid}", HandleAsync)
        .WithTags("Swimmers")
        .WithName("GetSwimmer")
        .WithDescription("Gets a swimmer by their unique identifier.")
        .Produces<GetSwimmerResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .AllowAnonymous();
    }

    /// <summary>
    /// Handles the incoming request to retrieve a swimmer by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the swimmer.</param>
    /// <param name="requestHandler">The request handler for retrieving a swimmer.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the request.</returns>
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
        else
        {
            return Results.NotFound();
        }
    }
}