using SwimTracker.Api.ProblemDetails.Endpoints;
using SwimTracker.Api.ProblemDetails.Endpoints.Clubs;
using SwimTracker.Api.ProblemDetails.Endpoints.Swimmers;
using SwimTracker.Api.ProblemDetails.Endpoints.Weatherforecast;

namespace SwimTracker.Api.ProblemDetails.Extensions;

/// <summary>
/// Extension methods for registering and mapping API endpoints.
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Registers all endpoint implementations with the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection to add endpoints to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        //Assembly assembly = Assembly.GetExecutingAssembly();

        //var endpointTypes = assembly.GetTypes()
        //    .Where(t => t.IsAssignableTo(typeof(IEndpoint)) && t.IsClass);

        //foreach (var type in endpointTypes)
        //{
        //    services.AddTransient(typeof(IEndpoint), type);
        //}

        //return services;

        services.AddTransient<IEndpoint, GetWeatherforecast>();
        services.AddTransient<IEndpoint, GetClub>();
        services.AddTransient<IEndpoint, GetClubs>();
        services.AddTransient<IEndpoint, CreateClub>();
        // Swimmers
        services.AddTransient<IEndpoint, GetSwimmer>();
        services.AddTransient<IEndpoint, GetSwimmers>();
        services.AddTransient<IEndpoint, CreateSwimmer>();

        return services;
    }

    /// <summary>
    /// Maps all registered endpoints to the application's routing pipeline.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <param name="routeGroupBuilder">Optional route group builder for grouping endpoints.</param>
    /// <returns>The application builder.</returns>
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