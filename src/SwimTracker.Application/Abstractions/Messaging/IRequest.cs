namespace SwimTracker.Application.Abstractions.Messaging;

/// <summary>
/// Represents a request with no return value, such as a command or a notification.
/// </summary>
public interface IRequest
{ }

/// <summary>
/// Represents a request that expects a response of type <typeparamref name="TResponse"/>, such as a query.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IRequest<TResponse>
{ }