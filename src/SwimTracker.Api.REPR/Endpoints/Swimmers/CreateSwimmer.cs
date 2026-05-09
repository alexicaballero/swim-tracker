using SwimTracker.Application.Swimmers.CreateSwimmer;

namespace SwimTracker.Api.REPR.Endpoints.Swimmers;

/// <summary>
/// Endpoint for creating a new swimmer in the system.
/// </summary>
public class CreateSwimmer : IEndpoint
{
    /// <summary>
    /// Maps the endpoint to the specified route builder.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/swimmers", HandleAsync)
        .WithTags("Swimmers")
        .WithDescription("Creates a new swimmer.")
        .WithName("CreateSwimmer")
        .Produces<CreateSwimmerResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .AllowAnonymous();
    }

    /// <summary>
    /// Handles the incoming request to create a new swimmer in the system.
    /// </summary>
    /// <param name="request">The request to create a new swimmer.</param>
    /// <param name="requestHandler">The request handler for creating a new swimmer.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The result of the request.</returns>
    private async Task<IResult> HandleAsync(
        CreateSwimmerRequest request,
        IRequestHandler<CreateSwimmerRequest, CreateSwimmerResponse> requestHandler,
        CancellationToken cancellationToken)
    {
        var result = await requestHandler.HandleAsync(request, cancellationToken);

        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }
        else
        {
            return Results.BadRequest(result.Error);
        }
    }
}