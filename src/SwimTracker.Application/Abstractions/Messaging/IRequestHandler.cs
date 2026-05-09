using SwimTracker.Application.Abstractions.Messaging;
using SwimTracker.SharedKernel;

/// <summary>
/// Defines a handler for processing requests of type <typeparamref name="TRequest"/> that do not
/// expect a response. The handler is responsible for executing the logic associated with the
/// request and returning a <see cref="Result"/> to indicate success or failure.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <returns>A <see cref="Result"/> indicating the success or failure of the request.</returns>
public interface IRequestHandler<TRequest> where TRequest : IRequest
{
    Task<Result> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a handler for processing requests of type <typeparamref name="TRequest"/> that expect a
/// response of type <typeparamref name="Result{TResponse}"/>. The handler is responsible for
/// executing the logic associated with the request and returning the appropriate response.
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
/// <returns>A <see cref="Result{TResponse}"/> containing the response data or an error message.</returns>
public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    Task<Result<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

/// <summary>
/// Defines a handler for processing requests that expect a response of type <typeparamref name="TResponse"/>. The handler is responsible for
/// executing the logic associated with the request and returning the appropriate response.
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IHandler<TResponse>
{
    Task<Result<TResponse>> HandleAsync(CancellationToken cancellationToken);
}