using SwimTracker.Api.ResultPattern.Extensions;
using SwimTracker.Application.Swimmers.GetSwimmers;

namespace SwimTracker.Api.ResultPattern.Endpoints.Swimmers;

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
        return result.ToHttpResult();
    }
}