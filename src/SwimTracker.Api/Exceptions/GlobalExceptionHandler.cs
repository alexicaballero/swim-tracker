using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics;

namespace SwimTracker.Api.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IHostEnvironment _environment;

    public GlobalExceptionHandler(
        IProblemDetailsService problemDetailsService,
        ILogger<GlobalExceptionHandler> logger,
        IHostEnvironment environment)
    {
        _problemDetailsService = problemDetailsService;
        _logger = logger;
        _environment = environment;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = exception switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            ApplicationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        Activity? activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;

        // Structured logging of the exception
        _logger.LogError(
            exception,
            "Unhandled exception occurred. TraceId: {TraceId}, RequestId: {RequestId}, Path: {Path}, Method: {Method}, StatusCode: {StatusCode}",
            activity?.TraceId.ToString() ?? "N/A",
            httpContext.TraceIdentifier,
            httpContext.Request.Path,
            httpContext.Request.Method,
            httpContext.Response.StatusCode);

        return await _problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
                {
                    Type = exception.GetType().FullName,
                    Status = httpContext.Response.StatusCode,
                    Title = "An unexpected error occurred.",
                    Detail = _environment.IsDevelopment()
                        ? exception.Message
                        : "An error occurred while processing your request."
                    // Instance and Extensions are added automatically in CustomizeProblemDetails
                }
            });
    }
}