using SwimTracker.Api.Endpoints;
using SwimTracker.Api.Endpoints.Clubs;
using SwimTracker.Api.Endpoints.Swimmers;
using SwimTracker.Api.Endpoints.Weatherforecast;

namespace SwimTracker.Api.Extensions;

public static class EndpointExtensions
{
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