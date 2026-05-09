using Microsoft.AspNetCore.Mvc;
using SwimTracker.Application.Swimmers.CreateSwimmer;

namespace SwimTracker.Api.Endpoints.Swimmers;

public class CreateSwimmer : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/swimmers", HandleAsync)
        .WithTags("Swimmers");
    }

    private async Task<IResult> HandleAsync(
        HttpContext context,
        [FromBody] CreateSwimmerRequest request,
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
            return Results.InternalServerError();
        }
    }
}