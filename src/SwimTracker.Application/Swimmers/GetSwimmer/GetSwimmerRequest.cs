using SwimTracker.Application.Abstractions.Messaging;

namespace SwimTracker.Application.Swimmers.GetSwimmer;

public record GetSwimmerRequest(Guid Id) : IRequest<GetSwimmerResponse>;