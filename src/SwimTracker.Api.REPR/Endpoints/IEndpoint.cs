namespace SwimTracker.Api.REPR.Endpoints;

/// <summary>
/// Interface for defining an API endpoint.
/// </summary>
public interface IEndpoint
{
    /// <summary>
    /// Maps the endpoint to the specified route builder.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    void MapEndpoint(IEndpointRouteBuilder app);
}