using FastEndpoints;
using FastEndpoints.Swagger;
using SwimTracker.Api.REPR.Extensions;
using SwimTracker.Application;
using SwimTracker.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpoints();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api");
app.UseSwaggerGen();

app.MapEndpoints();

app.Run();