using Microsoft.Extensions.DependencyInjection;
using SwimTracker.Application.Clubs.CreateClub;
using SwimTracker.Application.Clubs.GetClub;
using SwimTracker.Application.Clubs.GetClubs;
using SwimTracker.Application.Swimmers.CreateSwimmer;
using SwimTracker.Application.Swimmers.GetSwimmer;
using SwimTracker.Application.Swimmers.GetSwimmers;

namespace SwimTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRequestHandler<GetClubRequest, GetClubResponse>, GetClubHandler>();
        services.AddScoped<IRequestHandler<CreateClubRequest>, CreateClubHandler>();
        services.AddScoped<IHandler<List<GetClubsResponse>>, GetClubsHandler>();

        // Swimmers
        services.AddScoped<IRequestHandler<GetSwimmerRequest, GetSwimmerResponse>, GetSwimmerHandler>();
        services.AddScoped<IRequestHandler<CreateSwimmerRequest, CreateSwimmerResponse>, CreateSwimmerHandler>();
        services.AddScoped<IHandler<List<GetSwimmersResponse>>, GetSwimmersHandler>();

        return services;
    }
}