using Microsoft.AspNetCore.Http.Features;
using SwimTracker.Api.ProblemDetails.Exceptions;
using SwimTracker.Api.ProblemDetails.Extensions;
using SwimTracker.Application;
using SwimTracker.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configure Problem Details with custom options
builder.Services.AddProblemDetails(options =>
{
    // Customize the Problem Details response for ALL responses
    options.CustomizeProblemDetails = context =>
    {
        var httpContext = context.HttpContext;
        var activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;

        // Instance: URI that identifies the specific occurrence of the problem
        context.ProblemDetails.Instance ??= $"{httpContext.Request.Method} {httpContext.Request.Path}";

        // Traceability properties in Extensions
        context.ProblemDetails.Extensions["requestId"] = httpContext.TraceIdentifier;
        context.ProblemDetails.Extensions["traceId"] = activity?.TraceId.ToString() ?? "N/A";
        context.ProblemDetails.Extensions["timestamp"] = DateTime.UtcNow.ToString("O");

        // In development, include exception type for quick debugging
        if (builder.Environment.IsDevelopment() && context.Exception != null)
        {
            context.ProblemDetails.Extensions["exceptionType"] = context.Exception.GetType().FullName;
        }
    };
});

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddEndpoints();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapEndpoints();

app.UseExceptionHandler();

app.Run();