using SwimTracker.Application.Swimmers.GetSwimmers;

namespace SwimTracker.Api.REPR.Endpoints.Swimmers;

/// <summary>
/// Endpoint for retrieving a list of all swimmers in the system.
/// </summary>
public class GetSwimmers : IEndpoint
{
    /// <summary>
    /// Maps the endpoint to the specified route builder.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>   
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/swimmers", HandleAsync)
        .WithTags("Swimmers")
        .WithName("GetSwimmers")
        .WithDescription("Gets a list of all swimmers.")
        .AllowAnonymous()
        .Produces<List<GetSwimmersResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .AllowAnonymous();
    }

    /// <summary>
    /// Handles the incoming request to retrieve a list of all swimmers in the system.
    /// </summary>
    /// <param name="requestHandler">The request handler for retrieving swimmers.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the request.</returns>
    private async Task<IResult> HandleAsync(
        IHandler<List<GetSwimmersResponse>> requestHandler,
        CancellationToken cancellationToken)
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