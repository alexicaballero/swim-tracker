
/// <summary>
/// Defines a contract for mapping API endpoints to the application's route builder.
/// </summary>
namespace SwimTracker.Api.Endpoints;

/// <summary>
/// Interface for defining an endpoint that can be mapped to the application's routing system.
/// </summary>
public interface IEndpoint
{
    /// <summary>
    /// Maps the endpoint to the specified <see cref="IEndpointRouteBuilder"/>.
    /// </summary>
    /// <param name="app">The route builder to which the endpoint will be mapped.</param>
    void MapEndpoint(IEndpointRouteBuilder app);
}