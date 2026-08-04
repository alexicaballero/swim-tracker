using SwimTracker.Api.ResultPattern.Extensions;
using SwimTracker.Application.Swimmers.GetSwimmer;

namespace SwimTracker.Api.ResultPattern.Endpoints.Swimmers;

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
        return result.ToHttpResult();
    }
}