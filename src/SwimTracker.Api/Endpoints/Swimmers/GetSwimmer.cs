using SwimTracker.Application.Swimmers.GetSwimmer;

namespace SwimTracker.Api.Endpoints.Swimmers;

public class GetSwimmer : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/swimmers/{id:guid}", HandleAsync)
            .WithTags("Swimmers");
    }

    private async Task HandleAsync(
        HttpContext context, Guid id,
        IRequestHandler<GetSwimmerRequest, GetSwimmerResponse> requestHandler,
        CancellationToken cancellationToken)
    {
        var request = new GetSwimmerRequest(id);
        var result = await requestHandler.HandleAsync(request, cancellationToken);

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