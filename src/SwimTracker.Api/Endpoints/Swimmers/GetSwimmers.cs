using SwimTracker.Application.Swimmers.GetSwimmers;


/// <summary>
/// Endpoint for retrieving a list of swimmers.
/// </summary>
namespace SwimTracker.Api.Endpoints.Swimmers;

/// <summary>
/// Represents the endpoint for getting swimmers.
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
    /// Handles the GET swimmers request.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="requestHandler">The handler for the request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    private async Task HandleAsync(
        HttpContext context,
        IHandler<List<GetSwimmersResponse>> requestHandler,
        CancellationToken cancellationToken)
    {
        var result = await requestHandler.HandleAsync(cancellationToken);

        if (result.IsSuccess)
        {
            await context.Response.WriteAsJsonAsync(result.Value, cancellationToken);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }
    }
}