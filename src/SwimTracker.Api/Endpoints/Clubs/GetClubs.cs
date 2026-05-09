using SwimTracker.Application.Clubs.GetClubs;

namespace SwimTracker.Api.Endpoints.Clubs;

public class GetClubs : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/clubs", HandleAsync)
            .WithTags("Clubs");
    }

    private static async Task<IResult> HandleAsync(IHandler<List<GetClubsResponse>> requestHandler, CancellationToken cancellationToken)
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