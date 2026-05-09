using Microsoft.AspNetCore.Mvc;
using SwimTracker.Application.Clubs.CreateClub;

namespace SwimTracker.Api.Endpoints.Clubs;

public class CreateClub : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("api/clubs", async ([FromBody] CreateClubRequest request, IRequestHandler<CreateClubRequest> requestHandler, CancellationToken cancellationToken) =>
        {
            var result = await requestHandler.HandleAsync(request, cancellationToken);

            if (result.IsSuccess)
            {
                return Results.Created($"api/clubs/{request.Name}", request);
            }
            else
            {
                return Results.BadRequest(result.Error);
            }
        })
        .WithTags("Clubs");
    }
}