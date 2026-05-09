using SwimTracker.Application.Swimmers.GetSwimmers;

namespace SwimTracker.Api.Endpoints.Swimmers;

public class GetSwimmers : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/swimmers", HandleAsync)
            .WithTags("Swimmers");
    }

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