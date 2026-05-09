// using Microsoft.AspNetCore.Mvc;
// using SwimTracker.Application.Clubs.CreateClub;
// using SwimTracker.Application.Clubs.GetClub;
// using SwimTracker.Application.Clubs.GetClubs;

// namespace SwimTracker.Api.REPR.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// public class ClubsController : ControllerBase
// {
//     // [HttpGet("{id:guid}")]
//     // public async Task<IActionResult> GetClub(
//     //     Guid id,
//     //     IRequestHandler<GetClubRequest, ClubResponse> requestHandler,
//     //     CancellationToken cancellationToken)
//     // {
//     //     var request = new GetClubRequest(id);
//     //     var result = await requestHandler.HandleAsync(request, cancellationToken);

//     //     if (result.IsSuccess)
//     //     {
//     //         return Ok(result.Value);
//     //     }
//     //     else
//     //     {
//     //         return NotFound();
//     //     }
//     // }

//     // [HttpPost]
//     // public async Task<IActionResult> CreateClub(
//     //     [FromBody] CreateClubRequest request,
//     //     IRequestHandler<CreateClubRequest> requestHandler,
//     //     CancellationToken cancellationToken)
//     // {
//     //     var result = await requestHandler.HandleAsync(request, cancellationToken);

//     //     if (result.IsSuccess)
//     //     {
//     //         return Created($"api/clubs/{request.Name}", request);
//     //     }
//     //     else
//     //     {
//     //         return BadRequest(result.Error);
//     //     }
//     // }

//     // [HttpGet]
//     // public async Task<IActionResult> GetClubs(
//     //     IHandler<List<GetClubsResponse>> requestHandler,
//     //     CancellationToken cancellationToken)
//     // {
//     //     var result = await requestHandler.HandleAsync(cancellationToken);

//     //     if (result.IsSuccess)
//     //     {
//     //         return Ok(result.Value);
//     //     }
//     //     else
//     //     {
//     //         return NotFound();
//     //     }
//     // }
// }
