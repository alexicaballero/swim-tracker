// using Microsoft.AspNetCore.Mvc;
// using SwimTracker.Application.Swimmers.CreateSwimmer;
// using SwimTracker.Application.Swimmers.GetSwimmer;
// using SwimTracker.Application.Swimmers.GetSwimmers;

// namespace SwimTracker.Api.REPR.Controllers;

// [ApiController]
// [Route("api/[controller]")]
// public class SwimmersController : ControllerBase
// {
//     // [HttpGet("{id:guid}")]
//     // public async Task<IActionResult> GetSwimmer(
//     //     Guid id,
//     //     IRequestHandler<GetSwimmerRequest, GetSwimmerResponse> requestHandler,
//     //     CancellationToken cancellationToken)
//     // {
//     //     var request = new GetSwimmerRequest(id);
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
//     // public async Task<IActionResult> CreateSwimmer(
//     //     [FromBody] CreateSwimmerRequest request,
//     //     IRequestHandler<CreateSwimmerRequest, CreateSwimmerResponse> requestHandler,
//     //     CancellationToken cancellationToken)
//     {
//         var result = await requestHandler.HandleAsync(request, cancellationToken);

//         if (result.IsSuccess)
//         {
//             return Ok(result.Value);
//         }
//         else
//         {
//             return BadRequest(result.Error);
//         }
//     }

//     // [HttpGet]
//     // public async Task<IActionResult> GetSwimmers(
//     //     IHandler<List<GetSwimmersResponse>> requestHandler,
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