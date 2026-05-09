using SwimTracker.Api.REPR.Endpoints;
using SwimTracker.Api.REPR.Endpoints.Swimmers;

namespace SwimTracker.Api.REPR.Extensions;

/// <summary>
/// Extension methods for registering and mapping API endpoints.
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Registers the API endpoints with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        services.AddTransient<IEndpoint, GetSwimmers>();
        services.AddTransient<IEndpoint, GetSwimmer>();
        services.AddTransient<IEndpoint, CreateSwimmer>();

        return services;
    }

    /// <summary>
    /// Maps the registered API endpoints to the specified route builder.
    /// </summary>
    /// <param name="app">The application builder.</param>
    /// <param name="routeGroupBuilder">An optional route group builder.</param>
    /// <returns>The updated application builder.</returns>
    public static IApplicationBuilder MapEndpoints(
     this WebApplication app,
     RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;

        IEnumerable<IEndpoint> endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (IEndpoint endpoint in endpoints)
        {
            endpoint.MapEndpoint(builder);
        }

        return app;
    }
}