using SwimTracker.Application.Clubs.GetClub;

namespace SwimTracker.Api.Endpoints.Clubs;

public class GetClub : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("api/clubs/{id:guid}", HandleAsync)
            .WithTags("Clubs");
    }

    private static async Task<IResult> HandleAsync(Guid id, IRequestHandler<GetClubRequest, ClubResponse> requestHandler, CancellationToken cancellationToken)
    {
        var request = new GetClubRequest(id);
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